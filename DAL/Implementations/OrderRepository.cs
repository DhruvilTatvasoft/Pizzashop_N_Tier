using DAL.Data;
using Microsoft.IdentityModel.Tokens;

public class OrderRepository : IOrderRepository
{
    private readonly PizzashopCContext _context;

    public OrderRepository(PizzashopCContext context)
    {
        _context = context;
    }

    public List<Order> getAllOrderByDateFilter(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate)
    {
        List<Order> orders = _context.Orders.Where(order=>order.Createdat >= startDate && order.Createdat <= endDate).ToList();
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

    public List<Order> getAllOrderByOptionFilter(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate)
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


    public List<Order> getAllorders()
    {
        List<Order> orders = _context.Orders.Where(order => order.IsDeleted == false).ToList();
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

    public List<Order>? getAllordersBySearch(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate)
    {
        List<Order> orders = _context.Orders.Where(order => 
        order.Orderid == int.Parse(searchedOrder)).ToList();
        // List<Order> orders2 = _context.Orders.Where( order=>
        // (!status.HasValue || order.Statusid == status) &&
        // (!searchedOrder.IsNullOrEmpty() || order.Orderid == int.Parse(searchedOrder)) &&
        // (!filterBy.IsNullOrEmpty()|| )
        // )
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

public List<Order>? GetAllOrdersByFilters(int? status, string? searchedOrder, string? filterBy, DateTime? startDate, DateTime? endDate)
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

    List<Order> orders = query.ToList();

  
    var statusIds = orders.Select(o => o.Statusid).Distinct().ToList();
    var customerIds = orders.Select(o => o.Customerid).Distinct().ToList();

    var statusDict = _context.Orderstatuses
                            .Where(os => statusIds.Contains(os.Orderstatusid))
                            .ToDictionary(os => os.Orderstatusid);

    var customerDict = _context.Customers
                               .Where(c => customerIds.Contains(c.Customerid))
                               .ToDictionary(c => c.Customerid);

    foreach (var order in orders)
    {
        order.Status = statusDict.ContainsKey(order.Statusid) ? statusDict[order.Statusid] : null!;
        order.Customer = customerDict.ContainsKey(order.Customerid) ? customerDict[order.Customerid] : new Customer();
    }
    return orders;
}


    public List<Order> getAllOrdersFromStatus(int? status,string? searchedOrder,string? filterBy,DateTime? startDate,DateTime? endDate)
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
}