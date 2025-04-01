using DAL.Data;
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

    public List<Order>? GetAllOrdersByFilters(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate,int pageNumber,int pageSize,string sortOrder,string sortBy,bool fromExport)
    {
        IQueryable<Order> query = _context.Orders.AsQueryable();
        DateTime currentDate = DateTime.Now;

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
        else if(fromExport){
            query = query.Where(order=>order.IsDeleted == false);
            return query.ToList();
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
        
     if(sortBy == "orderId"){
            if(sortOrder == "desc"){
                query = query.OrderByDescending(o=>o.Orderid);
            }
            else{
                query = query.OrderBy(o=>o.Orderid);
            }
        }
        if(sortBy == "customerName"){
                if(sortOrder == "desc"){
                    query = query.OrderByDescending(order => order.Customer.Customername);
                }
                else{
                   query = query.OrderBy(order => order.Customer.Customername);
                }
        }
        if(sortBy == "Date"){
            if(sortOrder == "desc"){
                query = query.OrderByDescending(order => order.Createdat);
            }
            else{
                query = query.OrderBy(order => order.Createdat);
            }
        }
        else if(sortBy == "totalAmount"){
               if(sortOrder == "desc"){
                query = query.OrderByDescending(order => order.Totalamount);
            }
            else{
                query = query.OrderBy(order => order.Totalamount);
            }
        }
        List<Order> orders = query.Skip((pageNumber - 1) * pageSize)
                                    .Take(pageSize).ToList();
        return orders;
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
        return _context.Orders.Where(order=>order.IsDeleted == false).Count();
    }
}