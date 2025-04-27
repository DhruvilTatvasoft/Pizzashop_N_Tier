using DAL.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;


public class OrderRepository : IOrderRepository
{
    private readonly PizzashopCContext _context;

    public OrderRepository(PizzashopCContext context)
    {
        _context = context;
    }
    public OrderViewModel GetAllOrdersByFilters(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize, string sortOrder, string sortBy, bool fromExport)
    {
        IQueryable<Order> query = _context.Orders.AsQueryable();
        DateTime currentDate = DateTime.Now;
        OrderViewModel model = new OrderViewModel();

        if (!string.IsNullOrEmpty(searchedOrder) && int.TryParse(searchedOrder, out int searchOrderId))
        {
            query = query.Where(order => order.Orderid == searchOrderId);
        }

        if (status.HasValue && status.Value != 0)
        {
            query = query.Where(order => order.Statusid == status);
        }
        if (startDate.HasValue && endDate.HasValue)
        {
            query = query.Where(order => order.Createdat >= startDate && order.Createdat <= endDate);
        }
        else if (startDate.HasValue)
        {
            query = query.Where(order => order.Createdat >= startDate);
        }
        else if (endDate.HasValue)
        {
            query = query.Where(order => order.Createdat <= endDate);
        }
        else if (fromExport)
        {
            query = query.Where(order => order.IsDeleted == false);
            var StatusIds = query.Select(o => o.Statusid).Distinct().ToList();
            var CustomerIds = query.Select(o => o.Customerid).Distinct().ToList();

            var StatusDict = _context.Orderstatuses
                                    .Where(os => StatusIds.Contains(os.Orderstatusid))
                                    .ToDictionary(os => os.Orderstatusid);

            var CustomerDict = _context.Customers
                                       .Where(c => CustomerIds.Contains(c.Customerid))
                                       .ToDictionary(c => c.Customerid);

            foreach (var order in query)
            {
                order.Status = StatusDict.ContainsKey(order.Statusid) ? StatusDict[order.Statusid] : null!;
                order.Customer = CustomerDict.ContainsKey(order.Customerid) ? CustomerDict[order.Customerid] : new Customer();
            }
            model.orders = query.ToList();
            return model;
        }

        if (!string.IsNullOrEmpty(filterBy))
        {
            switch (filterBy)
            {
                case "Last 7 days":
                    query = query.Where(o => o.Createdat >= currentDate.AddDays(-7));
                    break;

                case "Last 30 days":
                    query = query.Where(o => o.Createdat >= currentDate.AddDays(-30));
                    break;

                case "Current Month":
                    query = query.Where(o => o.Createdat.HasValue &&
                                              o.Createdat.Value.Month == currentDate.Month &&
                                              o.Createdat.Value.Year == currentDate.Year);
                    break;
            }
        }

        var statusIds = query.Select(o => o.Statusid).Distinct().ToList();
        var customerIds = query.Select(o => o.Customerid).Distinct().ToList();

        var statusDict = _context.Orderstatuses
                                .Where(os => statusIds.Contains(os.Orderstatusid))
                                .ToDictionary(os => os.Orderstatusid);

        var customerDict = _context.Customers
                                   .Where(c => customerIds.Contains(c.Customerid))
                                   .ToDictionary(c => c.Customerid);

        foreach (var order in query)
        {
            order.Status = statusDict.ContainsKey(order.Statusid) ? statusDict[order.Statusid] : null!;
            order.Customer = customerDict.ContainsKey(order.Customerid) ? customerDict[order.Customerid] : new Customer();
        }

        if (sortBy == "orderId")
        {
            if (sortOrder == "desc")
            {
                query = query.OrderByDescending(o => o.Orderid);
            }
            else
            {
                query = query.OrderBy(o => o.Orderid);
            }
        }
        if (sortBy == "customerName")
        {
            if (sortOrder == "desc")
            {
                query = query.OrderByDescending(order => order.Customer.Customername);
            }
            else
            {
                query = query.OrderBy(order => order.Customer.Customername);
            }
        }
        if (sortBy == "Date")
        {
            if (sortOrder == "desc")
            {
                query = query.OrderByDescending(order => order.Createdat);
            }
            else
            {
                query = query.OrderBy(order => order.Createdat);
            }
        }
        else if (sortBy == "totalAmount")
        {
            if (sortOrder == "desc")
            {
                query = query.OrderByDescending(order => order.Totalamount);
            }
            else
            {
                query = query.OrderBy(order => order.Totalamount);
            }
        }

        model.PageNumber = pageNumber;
        model.PageSize = pageSize;
        model.TotalOrders = query.Count();
        model.sortBy = sortBy;
        model.sortOrder = sortOrder;
        List<Order> orders = query.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize).ToList();
        model.orders = orders;
        return model;
    }
    public List<Orderstatus> getAllStatus()
    {
        return _context.Orderstatuses.ToList();
    }

    public Order? GetOrderDetails(int orderid)
    {
        Order order = _context.Orders.FirstOrDefault(order => order.Orderid == orderid)!;
        order.Status = _context.Orderstatuses.FirstOrDefault(orderStatus => orderStatus.Orderstatusid == order.Statusid)!;
        order.Customer = _context.Customers.FirstOrDefault(customer => customer.Customerid == order.Customerid)!;
        order.Section = _context.Sections.FirstOrDefault(section => section.Sectionid == order.Sectionid)!;
        
        order.Table = _context.Tables.FirstOrDefault(table => table.Tableid == order.Tableid && table.Sectionid == order.Sectionid)!;
        return order;
    }

    

    public Dictionary<int,Dictionary<Item,List<Modifier>>> GetItemsAndModifiersForOrder2(int orderid)
    {
        Dictionary<int,Dictionary<Item,List<Modifier>>> itemsAndModifiers = new Dictionary<int, Dictionary<Item, List<Modifier>>>();
        Dictionary<Item,List<Modifier>> itemModifier = new Dictionary<Item, List<Modifier>>();
       var orderedItemsGrouped = _context.OrderItemModifiers
            .Where(oim => oim.Orderid == orderid)
            .GroupBy(oim => new { oim.ItemId, oim.Orderitemdetailid })
            .ToList();
        bool hasItems = false;
        var itemModifierList = new List<Dictionary<Item, List<Modifier>>>();
        int count = 0;
        foreach (var group in orderedItemsGrouped)
        {
            var itemId = group.Key.ItemId;
            var orderItemDetailId = group.Key.Orderitemdetailid;

            
            var dbItem = _context.Items.FirstOrDefault(i => i.Itemid == itemId);

            if (dbItem == null)
                continue;

           
            var item = new Item
            {
                Itemid = dbItem.Itemid,
                Itemname = dbItem.Itemname,
                Categoryid = dbItem.Categoryid,
                Itemrate = dbItem.Itemrate
                
            };

            var orderItem = _context.Orderitems.FirstOrDefault(oi => oi.Orderitemid == orderItemDetailId);
            if (orderItem == null)
                continue;

            var modifierIds = _context.OrderItemModifiers
                .Where(oim => oim.Orderitemdetailid == orderItemDetailId && oim.ItemId == itemId)
                .Select(oim => oim.Modifierid)
                .ToList();

            var modifiers = _context.Modifiers
                .Where(mod => modifierIds.Contains(mod.Modifierid) && mod.Isdeleted == false)
                .ToList();
            foreach(var modifier in modifiers){
                modifier.Modifierquantity = _context.OrderItemModifiers.FirstOrDefault(oim=>oim.Orderitemdetailid == orderItemDetailId && oim.ItemId == itemId && oim.Modifierid == modifier.Modifierid).ModifierQuantity?? 0;
            }
            item.Itemquantity = _context.Orderitems.FirstOrDefault(orderedItem=>orderedItem.Orderitemid == orderItemDetailId).Orderitemquantity;
            count++;
            itemsAndModifiers.Add(count,new Dictionary<Item, List<Modifier>> { { item, modifiers } });
            
        }
        return itemsAndModifiers;
    }

    public int GetTotalOrderCount()
    {
        return _context.Orders.Where(order => order.IsDeleted == false).Count();
    }

    public Dictionary<int, orderItemModifierViewModel> GetOrderByOptionFilterForKot()
    {
        List<int> orderIds = _context.Orders.Where(order => order.IsDeleted == false).Select(order => order.Orderid).ToList();
        Dictionary<int, orderItemModifierViewModel> model = new Dictionary<int, orderItemModifierViewModel>();
        foreach (var id in orderIds)
        {
            orderItemModifierViewModel orderItemModifierModel = new orderItemModifierViewModel();
            Dictionary<Item, List<Modifier>> modifiersForItem = new Dictionary<Item, List<Modifier>>();
            List<Item> itemList = new List<Item>();
            List<int> items = _context.Ordermodifiers.Where(orderModifier => orderModifier.Orderid == id).GroupBy(orderModifier => orderModifier.Itemid).Select(group => group.Key).ToList();
            foreach (var itemid in items)
            {
                Item item = _context.Items.FirstOrDefault(item => item.Itemid == itemid && item.Isdeleted == false)!;
                itemList.Add(item);
                List<int> modifierIds = _context.Ordermodifiers.Where(orderModifier => orderModifier.Itemid == itemid && orderModifier.Orderid == id && orderModifier.Isdeleted == false).Select(orderModifier => orderModifier.Modifierid).ToList();
                List<Modifier> modifiers = new List<Modifier>();
                foreach (var modifierid in modifierIds)
                {
                    Modifier modifier = _context.Modifiers.FirstOrDefault(modifier => modifier.Modifierid == modifierid && modifier.Isdeleted == false)!;
                    modifiers.Add(modifier);
                }
                modifiersForItem.Add(item, modifiers);
                orderItemModifierModel.modifiersForItem = modifiersForItem;
            }
            model.Add(id, orderItemModifierModel);
        }

        return model;
    }

    public tableAndsection getOrderSectionAndTableDetails(int orderId)
    {
        Order order = _context.Orders.FirstOrDefault(order => order.Orderid == orderId)!;
        string tableName = _context.Tables.FirstOrDefault(table => table.Tableid == order.Tableid && table.Sectionid == order.Sectionid)?.Tablename ?? string.Empty;
        string sectionName = _context.Sections.FirstOrDefault(section => section.Sectionid == order.Sectionid).Sectionname!;
        tableAndsection tableAndsection = new tableAndsection();
        tableAndsection.tableName = tableName;
        tableAndsection.sectionName = sectionName;
        tableAndsection tableAndSectionNames = new tableAndsection();
        tableAndSectionNames.sectionName = sectionName;
        tableAndSectionNames.tableName = tableName;
        return tableAndSectionNames;
    }

   public Dictionary<Order, List<Dictionary<Item, List<Modifier>>>> GetOrderDetailsByCategory(int categoryid, bool? IsReady)
    {
    var orderIds = _context.Orders
        .Where(order => order.IsDeleted == false)
        .Select(order => order.Orderid)
        .ToList();

    var model = new Dictionary<Order, List<Dictionary<Item, List<Modifier>>>>();

    foreach (var orderId in orderIds)
    {
        var orderedItemsGrouped = _context.OrderItemModifiers
            .Where(oim => oim.Orderid == orderId)
            .GroupBy(oim => new { oim.ItemId, oim.Orderitemdetailid })
            .ToList();

        bool hasItems = false;
        var itemModifierList = new List<Dictionary<Item, List<Modifier>>>();

        foreach (var group in orderedItemsGrouped)
        {
            var itemId = group.Key.ItemId;
            var orderItemDetailId = group.Key.Orderitemdetailid;

            
            var dbItem = categoryid != 0
                ? _context.Items.FirstOrDefault(i => i.Itemid == itemId && i.Categoryid == categoryid)
                : _context.Items.FirstOrDefault(i => i.Itemid == itemId);

            if (dbItem == null)
                continue;

           
            var item = new Item
            {
                Itemid = dbItem.Itemid,
                Itemname = dbItem.Itemname,
                Categoryid = dbItem.Categoryid,
                
            };

            var orderItem = _context.Orderitems.FirstOrDefault(oi => oi.Orderitemid == orderItemDetailId);
            if (orderItem == null)
                continue;

            if (IsReady == null)
            {
                item.Itemquantity = orderItem.Orderitemquantity;
            }
            else if (IsReady == true && orderItem.Readyitemquanitiy > 0)
            {
                item.Itemquantity = orderItem.Readyitemquanitiy;
            }
            else if (IsReady == false && (orderItem.Orderitemquantity - orderItem.Readyitemquanitiy) > 0)
            {
                item.Itemquantity = orderItem.Orderitemquantity - orderItem.Readyitemquanitiy;
            }
            else
            {
                continue; 
            }

            var modifierIds = _context.OrderItemModifiers
                .Where(oim => oim.Orderitemdetailid == orderItemDetailId && oim.ItemId == itemId)
                .Select(oim => oim.Modifierid)
                .ToList();

            var modifiers = _context.Modifiers
                .Where(mod => modifierIds.Contains(mod.Modifierid) && mod.Isdeleted == false)
                .ToList();

            itemModifierList.Add(new Dictionary<Item, List<Modifier>> { { item, modifiers } });
            hasItems = true;
        }

        if (hasItems)
        {
            Order order = _context.Orders.FirstOrDefault(o => o.Orderid == orderId)!;
            model.Add(order, itemModifierList);
        }
    }
    return model;
}


    public SingleOrderDetailModel getSingleOrderDetail(int categoryid, int orderid)
    {
        SingleOrderDetailModel model = new SingleOrderDetailModel();
        model.orderid = orderid;
        var orderedItemsGrouped = _context.OrderItemModifiers
            .Where(oim => oim.Orderid == orderid)
            .GroupBy(oim => new { oim.ItemId, oim.Orderitemdetailid })
            .ToList();
        List<Dictionary<Item, List<Modifier>>> itemModifierList = new List<Dictionary<Item, List<Modifier>>>();
        foreach (var group in orderedItemsGrouped)
        {
            var itemId = group.Key.ItemId;
            var orderItemDetailId = group.Key.Orderitemdetailid;

            
            var dbItem = categoryid != 0
                ? _context.Items.FirstOrDefault(i => i.Itemid == itemId && i.Categoryid == categoryid)
                : _context.Items.FirstOrDefault(i => i.Itemid == itemId);

            if (dbItem == null)
                continue;


            var orderItem = _context.Orderitems.FirstOrDefault(oi => oi.Orderitemid == orderItemDetailId);
            if (orderItem == null)
                continue;
            
            var item = new Item
            {
                Itemid = orderItem.Orderitemid,
                Itemname = dbItem.Itemname,
                Categoryid = dbItem.Categoryid,
            };
           
            var modifierIds = _context.OrderItemModifiers
                .Where(oim => oim.Orderitemdetailid == orderItemDetailId && oim.ItemId == itemId)
                .Select(oim => oim.Modifierid)
                .ToList();

            var modifiers = _context.Modifiers
                .Where(mod => modifierIds.Contains(mod.Modifierid) && mod.Isdeleted == false)
                .ToList();

                
            item.Itemquantity = orderItem.Orderitemquantity-orderItem.Readyitemquanitiy;

            itemModifierList.Add(new Dictionary<Item, List<Modifier>> { { item, modifiers } });
        }
        model.itemAndModifiers = itemModifierList
            .SelectMany(dict => dict)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
            return model;        
    }
   public void changeReadyQuantity(Dictionary<int, int> readyItemCount){
        foreach (var pair in readyItemCount){
            var orderedItem = _context.Orderitems.FirstOrDefault(orderedItem=>orderedItem.Orderitemid == pair.Key);
            orderedItem!.Readyitemquanitiy = pair.Value;
            _context.Orderitems.Update(orderedItem);
        }
        _context.SaveChanges();
    }

    public int CreateOrder(int tokenid, int tableid)
    {
        Order newOrder = new Order();
        newOrder.Tableid = tableid;
        newOrder.Sectionid = _context.Tables.FirstOrDefault(table=>table.Tableid == tableid).Sectionid;
        newOrder.Customerid = _context.Waitingtokens.FirstOrDefault(token=>token.Waitingtokenid == tokenid).Customerid;
        newOrder.Totalpersons = _context.Waitingtokens.FirstOrDefault(token=>token.Waitingtokenid == tokenid).Totalpersons;
        newOrder.Createdat = DateTime.Now;
        newOrder.Createdby = 1;
        newOrder.Modifiedby = 1;
        _context.Orders.Add(newOrder);
        _context.SaveChanges();
        return newOrder.Orderid;
    }

    public void addItemInOrder(int itemid, List<int> modifiers)
    {
        
    }
}