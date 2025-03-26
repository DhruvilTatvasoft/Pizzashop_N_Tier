using DAL.Data;

public class OrderRepository : IOrderRepository
{
    private readonly PizzashopCContext _context;

    public OrderRepository(PizzashopCContext context)
    {
        _context = context;
    }

    public List<Order> getAllOrderByOptionFilter(string filterBy)
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

    public List<Order>? getAllordersBySearch(string searchedOrder)
    {
        List<Order> orders = _context.Orders.Where(order => order.Orderid == int.Parse(searchedOrder)).ToList();
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

    public List<Order> getAllOrdersFromStatus(int statusid)
    {


        List<Order> orders = _context.Orders.Where(order => order.Statusid == statusid && order.IsDeleted == false).ToList();
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