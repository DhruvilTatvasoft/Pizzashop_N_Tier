using DAL.Data;
using Microsoft.EntityFrameworkCore;


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
                order.Status = StatusDict.ContainsKey(order.Statusid ?? 0) ? StatusDict[order.Statusid ?? 0] : null!;
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
            order.Status = statusDict.ContainsKey(order.Statusid ?? 0) ? statusDict[order.Statusid ?? 0] : null!;
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
        return order;
    }



    public Dictionary<int, Dictionary<Item, List<Modifier>>> GetItemsAndModifiersForOrder2(int orderid)
    {
        Dictionary<int, Dictionary<Item, List<Modifier>>> itemsAndModifiers = new Dictionary<int, Dictionary<Item, List<Modifier>>>();
        Dictionary<Item, List<Modifier>> itemModifier = new Dictionary<Item, List<Modifier>>();
        var orderedItemsGrouped = _context.Orderitems
             .Where(oim => oim.Orderid == orderid)
             .ToList();
        bool hasItems = false;
        var itemModifierList = new List<Dictionary<Item, List<Modifier>>>();
        int count = 0;
        foreach (var group in orderedItemsGrouped)
        {
            var itemId = group.Itemid;
            var orderItemDetailId = group.Orderitemid;
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
            foreach (var modifier in modifiers)
            {
                modifier.Modifierquantity = _context.OrderItemModifiers.FirstOrDefault(oim => oim.Orderitemdetailid == orderItemDetailId && oim.ItemId == itemId && oim.Modifierid == modifier.Modifierid).ModifierQuantity ?? 0;
            }
            item.Itemquantity = _context.Orderitems.FirstOrDefault(orderedItem => orderedItem.Orderitemid == orderItemDetailId)?.Orderitemquantity ?? 0;
            count++;
            itemsAndModifiers.Add(count, new Dictionary<Item, List<Modifier>> { { item, modifiers } });

        }
        return itemsAndModifiers;
    }

    public int GetTotalOrderCount()
    {
        return _context.Orders.Where(order => order.IsDeleted == false).Count();
    }

    public Dictionary<int, orderItemModifierViewModel> GetOrderByOptionFilterForKot()
    {
        List<int> orderIds = _context.Orders.Where(order => order.IsDeleted == false && order.Statusid == 4).Select(order => order.Orderid).ToList();
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
        int tableid = _context.Ordertables.FirstOrDefault(orderTable => orderTable.Orderid == orderId).Tableid;
        Table OrderedTable = _context.Tables.FirstOrDefault(orderedTable => orderedTable.Tableid == tableid);
        string tableName = OrderedTable.Tablename;
        string sectionName = _context.Sections.FirstOrDefault(section => section.Sectionid == OrderedTable.Sectionid).Sectionname!;
        tableAndsection tableAndsection = new tableAndsection();
        tableAndsection.tableName = tableName;
        tableAndsection.sectionName = sectionName;
        return tableAndsection;
    }

    public void GetOrderDetailsByCategory(int categoryid, bool? IsReady, int pageSize, int pageNumber, KotViewModel kotModel)
    {
        var orderIds = _context.Orders
            .Where(order => order.IsDeleted == false && order.Statusid == 4 || order.Statusid == 1)
            .Select(order => order.Orderid)
            .ToList();


        var model = new Dictionary<Order, List<Dictionary<Item, List<Modifier>>>>();

        foreach (var orderId in orderIds)
        {
            bool hasItems = false;
            List<Orderitem> orderedItems = _context.Orderitems.Where(orderedItem => orderedItem.Orderid == orderId).ToList();
            var itemModifierList = new List<Dictionary<Item, List<Modifier>>>();
            bool isAllItemReady = true;
            foreach (var Ordereditem in orderedItems)
            {
                var dbItem = categoryid != 0
                     ? _context.Items.FirstOrDefault(i => i.Itemid == Ordereditem.Itemid && i.Categoryid == categoryid)
                     : _context.Items.FirstOrDefault(i => i.Itemid == Ordereditem.Itemid);

                bool addItem = true;

                if (dbItem == null)
                {
                    hasItems = false;
                }
                else
                {
                    var newItem = new Item
                    {
                        Itemname = dbItem.Itemname,
                        Itemid = dbItem.Itemid,
                        Itemquantity = 0
                    };
                    if (IsReady == null)
                    {
                        newItem.Itemquantity = Ordereditem.Orderitemquantity ?? 0;
                    }
                    else if (IsReady == true)
                    {
                        newItem.Itemquantity = Ordereditem.Readyitemquanitiy ?? 0;
                        if (Ordereditem.Readyitemquanitiy != Ordereditem.Orderitemquantity)
                        {
                            isAllItemReady = false;
                        }
                    }
                    else
                    {
                        newItem.Itemquantity = (Ordereditem.Orderitemquantity ?? 0) - (Ordereditem.Readyitemquanitiy ?? 0);
                    }
                    if (newItem.Itemquantity <= 0)
                    {
                        addItem = false;
                    }
                    if (addItem)
                    {
                        hasItems = true;
                        List<Modifier> modifiers = new List<Modifier>();
                        List<OrderItemModifier> modifiersForItem = _context.OrderItemModifiers.Where(orderedItemModifier => orderedItemModifier.ItemId == Ordereditem.Itemid && orderedItemModifier.Orderitemdetailid == Ordereditem.Orderitemid).ToList();
                        foreach (var modifier in modifiersForItem)
                        {
                            Modifier m = _context.Modifiers.FirstOrDefault(Modifier => Modifier.Modifierid == modifier.Modifierid);
                            modifiers.Add(m);
                        }
                        Dictionary<Item, List<Modifier>> itemAndModifiers = new Dictionary<Item, List<Modifier>>();
                        itemAndModifiers.Add(newItem, modifiers);
                        itemModifierList.Add(itemAndModifiers);
                    }
                }
            }
            if (hasItems)
            {
                Order order = _context.Orders.FirstOrDefault(o => o.Orderid == orderId)!;
                if (IsReady == true)
                {
                    if (isAllItemReady == true)
                    {
                        model.Add(order, itemModifierList);
                    }
                }
                else
                {
                    model.Add(order, itemModifierList);
                }
            }
        }
        kotModel.pageNumber = pageNumber;
        kotModel.pageSize = pageSize;
        kotModel.totalOrders = model.Count;
        model.OrderBy(order => order.Key.Orderid);
        kotModel.orderDetails = model.Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToDictionary(pair => pair.Key, pair => pair.Value);

    }


    public SingleOrderDetailModel getSingleOrderDetail(int categoryid, int orderid, string status)
    {
        SingleOrderDetailModel model = new SingleOrderDetailModel();
        model.orderid = orderid;
        var orderedItemsGrouped = _context.Orderitems
            .Where(oim => oim.Orderid == orderid)
            .ToList();
        List<Dictionary<Item, List<Modifier>>> itemModifierList = new List<Dictionary<Item, List<Modifier>>>();
        foreach (var group in orderedItemsGrouped)
        {
            var itemId = group.Itemid;
            var orderItemDetailId = group.Orderitemid;
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
            if (status == "In Progress")
            {
                item.Itemquantity = (orderItem.Orderitemquantity ?? 0) - (orderItem.Readyitemquanitiy ?? 0);
            }
            else
            {
                item.Itemquantity = orderItem.Readyitemquanitiy ?? 0;
            }
            itemModifierList.Add(new Dictionary<Item, List<Modifier>> { { item, modifiers } });
        }
        model.itemAndModifiers = itemModifierList
            .SelectMany(dict => dict)
            .ToDictionary(pair => pair.Key, pair => pair.Value);
        return model;
    }
    public void changeReadyQuantity(Dictionary<int, int> readyItemCount, string currentStatus)
    {
        foreach (var pair in readyItemCount)
        {
            var orderedItem = _context.Orderitems.FirstOrDefault(orderedItem => orderedItem.Orderitemid == pair.Key);
            if (currentStatus == "In Progress")
            {
                orderedItem!.Readyitemquanitiy = orderedItem!.Readyitemquanitiy + pair.Value;
            }
            else
            {
                orderedItem!.Readyitemquanitiy = orderedItem!.Readyitemquanitiy - pair.Value;
            }
            _context.Orderitems.Update(orderedItem);
        }
        _context.SaveChanges();
    }

    public int CreateOrder(int tokenid, int tableid)
    {
        Order newOrder = new Order();
        newOrder.Tableid = tableid;
        newOrder.Sectionid = _context.Tables.FirstOrDefault(table => table.Tableid == tableid).Sectionid;
        newOrder.Customerid = _context.Waitingtokens.FirstOrDefault(token => token.Waitingtokenid == tokenid).Customerid;
        newOrder.Totalpersons = _context.Waitingtokens.FirstOrDefault(token => token.Waitingtokenid == tokenid).Totalpersons;
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

    public int CreateOrderForCustomer(int tokenid, List<int> tableids)
    {
        Order order = new Order();
        Waitingtoken token = _context.Waitingtokens.FirstOrDefault(tkn => tkn.Waitingtokenid == tokenid && tkn.Isdeleted == false);
        order.Customerid = token.Customerid;
        order.Totalpersons = token.Totalpersons;
        order.Createdat = DateTime.Now;
        order.Createdby = 1;
        order.Modifiedby = 1;
        order.Modifiedat = DateTime.Now;
        order.Sectionid = token.Sectionid;
        order.IsDeleted = false;
        _context.Orders.Add(order);
        _context.SaveChanges();
        foreach (var tableid in tableids)
        {
            Table table = _context.Tables.FirstOrDefault(Table => Table.Tableid == tableid);
            table.Status = false;
            table.Statusname = "Assigned";
            Ordertable orderedtable = new Ordertable();
            orderedtable.Orderid = order.Orderid;
            orderedtable.Customerid = token.Customerid;
            orderedtable.Createdby = 1;
            orderedtable.Modifiedby = 1;
            orderedtable.Tableid = tableid;
            _context.Ordertables.Add(orderedtable);
            _context.Tables.Update(table);
        }
        _context.SaveChanges();
        return order.Orderid;
    }

    public void getAppliedTaxesForOrder(OrderViewModel model)
    {
        List<appliedTaxDetails> taxDetails = new List<appliedTaxDetails>();
        List<Ordertaxesandfee> taxDetailsForOrder = _context.Ordertaxesandfees.Where(orderTaxes => orderTaxes.Orderid == model.orderid).ToList();
        foreach (var tax in taxDetailsForOrder)
        {
            appliedTaxDetails taxDetails1 = new appliedTaxDetails();
            taxDetails1.taxname = tax.Taxname;
            taxDetails1.taxPercentage = (float)(tax.TaxPercentage);
            taxDetails1.taxtype = (tax.Taxtype);
            taxDetails.Add(taxDetails1);
        }
        model.appliedTaxDetails = taxDetails;
    }
}