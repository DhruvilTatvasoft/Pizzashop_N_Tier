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

    public List<Order> getAllOrderByDateFilter(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate)
    {
        List<Order> orders = _context.Orders.Where(order => order.Createdat >= startDate && order.Createdat <= endDate).ToList();
        foreach (var order in orders)
        {
            order.Status = _context.Orderstatuses.FirstOrDefault(orderStatus => orderStatus.Orderstatusid == order.Statusid)!;
        }
        foreach (var CurrentOrder in orders)
        {
            CurrentOrder.Customer = _context.Customers.FirstOrDefault(order => order.Customerid == CurrentOrder.Customerid) ?? new Customer();
        }
        return orders;
    }

    public List<Order> getAllOrderByOptionFilter(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate)
    {
        DateTime currentDate = DateTime.Now;
        IQueryable<Order> query = _context.Orders.AsQueryable();
        List<Order> orders = new List<Order>();
        switch (filterBy)
        {
            case "Last 7 days":
                query = query.Where(o => o.Createdat >= currentDate.AddDays(-7));
                break;

            case "Last 30 days":
                query = query.Where(o => o.Createdat != null && o.Createdat >= currentDate.AddDays(-30));
                break;

            case "Current Month":
                query = query.Where(o => o.Createdat.HasValue &&
                                          o.Createdat.Value.Month == currentDate.Month &&
                                          o.Createdat.Value.Year == currentDate.Year);
                break;

            default:
                break;
        }

        orders = query.ToList();
        foreach (var order in orders)
        {
            order.Status = _context.Orderstatuses.FirstOrDefault(orderStatus => orderStatus.Orderstatusid == order.Statusid)!;
        }
        foreach (var CurrentOrder in orders)
        {
            CurrentOrder.Customer = _context.Customers.FirstOrDefault(order => order.Customerid == CurrentOrder.Customerid) ?? new Customer();
        }
        return orders;
    }


    public List<Order> getAllorders(int pageNumber, int pageSize)
    {

        List<Order> orders = _context.Orders.Where(order => order.IsDeleted == false)
                   .Skip((pageNumber - 1) * pageSize)
                   .Take(pageSize)
                   .ToList();

        foreach (var order in orders)
        {
            order.Status = _context.Orderstatuses.FirstOrDefault(orderStatus => orderStatus.Orderstatusid == order.Statusid)!;
        }
        foreach (var CurrentOrder in orders)
        {
            CurrentOrder.Customer = _context.Customers.FirstOrDefault(order => order.Customerid == CurrentOrder.Customerid) ?? new Customer();
        }
        return orders;
    }

    public List<Order>? getAllordersBySearch(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate)
    {
        List<Order> orders = _context.Orders.Where(order =>
        order.Orderid == int.Parse(searchedOrder)).ToList();
        foreach (var order in orders)
        {
            order.Status = _context.Orderstatuses.FirstOrDefault(orderStatus => orderStatus.Orderstatusid == order.Statusid)!;
        }
        foreach (var CurrentOrder in orders)
        {
            CurrentOrder.Customer = _context.Customers.FirstOrDefault(order => order.Customerid == CurrentOrder.Customerid) ?? new Customer();
        }
        return orders;
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


    public List<Order> getAllOrdersFromStatus(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate)
    {


        List<Order> orders = _context.Orders.Where(order => order.Statusid == status && order.IsDeleted == false).ToList();
        foreach (var order in orders)
        {
            order.Status = _context.Orderstatuses.FirstOrDefault(orderStatus => orderStatus.Orderstatusid == order.Statusid)!;
        }
        foreach (var CurrentOrder in orders)
        {
            CurrentOrder.Customer = _context.Customers.FirstOrDefault(order => order.Customerid == CurrentOrder.Customerid) ?? new Customer();
        }
        return orders;
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
        order.Table = _context.Tables.FirstOrDefault(table => table.Tableid == orderid && table.Sectionid == order.Sectionid)!;
        return order;
    }

    public Dictionary<Item, List<Modifier>> GetItemsAndModifiersForOrder(int orderid)
    {
        decimal subtotal = 0;
        List<int> itemIds = _context.Ordermodifiers
                                    .Where(orderModifier => orderModifier.Orderid == orderid)
                                    .Select(orderModifier => orderModifier.Itemid).Distinct()
                                    .ToList();
        List<Item> items = new List<Item>();
        foreach (var id in itemIds)
        {
            List<Item> currentItem = _context.Items.Where(item => item.Itemid == id && item.Isdeleted == false).ToList();
            items.AddRange(currentItem);
        }
        Dictionary<Item, List<Modifier>> modifiersForItem = new Dictionary<Item, List<Modifier>>();

        foreach (var item in items)
        {
            List<Modifier> modifierForCurrItem = new List<Modifier>();
            List<int> modifierIdsForitem = _context.Ordermodifiers.Where(orderModifier => orderModifier.Orderid == orderid && orderModifier.Itemid == item.Itemid).Select(orderModifier => orderModifier.Modifierid).ToList();
            foreach (var id in modifierIdsForitem)
            {
                List<Modifier> modifierList = _context.Modifiers.Where(modifier => modifier.Modifierid == id && modifier.Isdeleted == false).ToList();
                foreach (var modifier in modifierList)
                {
                    modifier.Modifierquantity = _context.Ordermodifiers.FirstOrDefault(orderModifier => orderModifier.Modifierid == modifier.Modifierid && orderModifier.Orderid == orderid)!.Ordermodifierquantity;
                    subtotal = subtotal + modifier.Modifierquantity * modifier.Modifierrate;
                }
                modifierForCurrItem.AddRange(modifierList);
            }
            var orderItem = _context.Ordermodifiers.FirstOrDefault(orderItem => orderItem.Itemid == item.Itemid && orderItem.Orderid == orderid);
            item.Itemquantity = orderItem?.Orderitemquantity ?? 0;
            subtotal = subtotal + item.Itemquantity * item.Itemrate;
            modifiersForItem.Add(item, modifierForCurrItem);
        }
        return modifiersForItem;
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

    public Dictionary<int, tableAndsection> getOrderSectionAndTableDetails(int orderId)
    {
        Order order = _context.Orders.FirstOrDefault(order => order.Orderid == orderId)!;
        string tableName = _context.Tables.FirstOrDefault(table => table.Tableid == order.Tableid && table.Sectionid == order.Sectionid).Tablename!;
        string sectionName = _context.Sections.FirstOrDefault(section => section.Sectionid == order.Sectionid).Sectionname!;
        tableAndsection tableAndsection = new tableAndsection();
        tableAndsection.tableName = tableName;
        tableAndsection.sectionName = sectionName;
        Dictionary<int, tableAndsection> orderTableAndSectionDetails = new Dictionary<int, tableAndsection>();
        orderTableAndSectionDetails.Add(orderId, tableAndsection);
        return orderTableAndSectionDetails;
    }

    public Dictionary<int, orderItemModifierViewModel> GetOrderDetailsByCategory(int categoryid,bool? IsReady)
    {

        GetOrderDetailsByCategory2(categoryid,IsReady);
        List<int> orderIds = _context.Orders.Where(order => order.IsDeleted == false).Select(order => order.Orderid).ToList();
        Dictionary<int, orderItemModifierViewModel> model = new Dictionary<int, orderItemModifierViewModel>();
        foreach (var id in orderIds)
        {
            orderItemModifierViewModel orderItemModifierModel = new orderItemModifierViewModel();
            List<Item> itemList = new List<Item>();
            List<int> items = new List<int>();
            if(IsReady != null){
                items = _context.Ordermodifiers.Where(orderModifier => orderModifier.Orderid == id && orderModifier.IsReady == IsReady).GroupBy(orderModifier => orderModifier.Itemid).Select(group => group.Key).ToList();
            }
            else{
                items = _context.Ordermodifiers.Where(orderModifier => orderModifier.Orderid == id).GroupBy(orderModifier => orderModifier.Itemid).Select(group => group.Key).ToList();
            }
            bool flag = false;
            foreach (var itemid in items)
            {
            Dictionary<Item, List<Modifier>> modifiersForItem = new Dictionary<Item, List<Modifier>>();
                Item? item = new Item();
                if(categoryid != 0){
                    item = _context.Items.FirstOrDefault(item => item.Itemid == itemid && item.Isdeleted == false && item.Categoryid == categoryid);
                }
                else{
                    item = _context.Items.FirstOrDefault(item => item.Itemid == itemid && item.Isdeleted == false);
                }
                if (item != null)
                {
                
                    flag = true;
                    itemList.Add(item);
                    List<int> modifierIds = _context.Ordermodifiers.Where(orderModifier => orderModifier.Itemid == itemid && orderModifier.Orderid == id && orderModifier.Isdeleted == false).Select(orderModifier => orderModifier.Modifierid).ToList();
                    List<Modifier> modifiers = new List<Modifier>();
                    foreach (var modifierid in modifierIds)
                    {
                        Modifier modifier = _context.Modifiers.FirstOrDefault(modifier => modifier.Modifierid == modifierid && modifier.Isdeleted == false)!;
                        modifiers.Add(modifier);
                    }
                    modifiersForItem.Add(LoadQuantitiesForItem(item,id), modifiers);
                    orderItemModifierModel.modifiersForItem = modifiersForItem;
                }
            }
            if(flag == true){
            model.Add(id, orderItemModifierModel);
            }
        }
        return model;
    }
    public Dictionary<int, orderItemModifierViewModel> GetOrderDetailsByCategory2(int categoryid,bool? IsReady)
    {
        List<int> orderIds = _context.Orders.Where(order => order.IsDeleted == false).Select(order => order.Orderid).ToList();
        Dictionary<int, orderItemModifierViewModel> model = new Dictionary<int, orderItemModifierViewModel>();
        foreach (var id in orderIds)
        {
            orderItemModifierViewModel orderItemModifierModel = new orderItemModifierViewModel();
            List<Item> itemList = new List<Item>();
            List<int> items = new List<int>();
             List<OrderItemModifier> orderedItems = new List<OrderItemModifier>();
            // if(IsReady != null){
            //    var  orderedItems1= _context.OrderItemModifiers
            //     .Where(orderItemModifiers => orderItemModifiers.Orderid == id)
            //     .GroupBy(orderItemModifiers => new { orderItemModifiers.ItemId, orderItemModifiers.Orderitemdetailid })
            //     .ToList();            }
            // else{
             var orderedItems2 = _context.OrderItemModifiers
                .Where(orderItemModifiers => orderItemModifiers.Orderid == id)
                .GroupBy(orderItemModifiers => new { orderItemModifiers.ItemId, orderItemModifiers.Orderitemdetailid })
                .ToList();
            // }

            foreach(var orderditem in orderedItems2){
                var itemid = orderditem.Key.ItemId;
                var Orderitemdetailid = orderditem.Key.Orderitemdetailid;
                var modifiers = _context.OrderItemModifiers.Where(oim => oim.Orderitemdetailid == Orderitemdetailid).Select(oim=>oim.Modifierid).ToList();
                
                
            }
            
            bool flag = false;
            foreach (var itemid in items)
            {
            Dictionary<Item, List<Modifier>> modifiersForItem = new Dictionary<Item, List<Modifier>>();
                Item? item = new Item();
                if(categoryid != 0){
                    item = _context.Items.FirstOrDefault(item => item.Itemid == itemid && item.Isdeleted == false && item.Categoryid == categoryid);
                }
                else{
                    item = _context.Items.FirstOrDefault(item => item.Itemid == itemid && item.Isdeleted == false);
                }
                if (item != null)
                {
                
                    flag = true;
                    itemList.Add(item);
                    List<int> modifierIds = _context.Ordermodifiers.Where(orderModifier => orderModifier.Itemid == itemid && orderModifier.Orderid == id && orderModifier.Isdeleted == false).Select(orderModifier => orderModifier.Modifierid).ToList();
                    List<Modifier> modifiers = new List<Modifier>();
                    foreach (var modifierid in modifierIds)
                    {
                        Modifier modifier = _context.Modifiers.FirstOrDefault(modifier => modifier.Modifierid == modifierid && modifier.Isdeleted == false)!;
                        modifiers.Add(modifier);
                    }
                    modifiersForItem.Add(LoadQuantitiesForItem(item,id), modifiers);
                    orderItemModifierModel.modifiersForItem = modifiersForItem;
                }
            }
            if(flag == true){
            model.Add(id, orderItemModifierModel);
            }
        }
        return model;
    }

    public Item LoadQuantitiesForItem(Item item,int orderid){
        List<Ordermodifier> items= _context.Ordermodifiers.Where(om => om.Itemid == item.Itemid && om.Orderid == orderid ).ToList();
        int itemQuantity = 0;
        foreach (var item1 in items){
            itemQuantity += item1.Orderitemquantity ?? 0;
        }
        item.Itemquantity = itemQuantity;
        return item;
    }

}