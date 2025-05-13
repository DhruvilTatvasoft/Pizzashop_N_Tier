using DAL.Data;
using DAL.interfaces;

public class CustomerRepository : ICustomerRepository
{
    public PizzashopCContext _context;

    public CustomerRepository(PizzashopCContext context)
    {
        _context = context;
    }
    public CustomerViewModel getAllCustomers(int pageSize, int pageNumber, string sortBy, string sortOrder, string? search, string filterBy, DateTime? startDate, DateTime? endDate,bool? isExport = false)
    {
        CustomerViewModel model = new CustomerViewModel();

        List<Customer> allCustomers = _context.Customers.Where(customer => customer.Isdeleted == false).ToList();
        DateTime currentDate = DateTime.Now;

        foreach (var customer in allCustomers)
        {
            
                customer.Totalorders = _context.Orders.Where(order => order.Customerid == customer.Customerid && order.IsDeleted == false).Count();
                _context.Customers.Update(customer);
                _context.SaveChanges();
        }
        IQueryable<Customer> query;
        query = _context.Customers.Where(customer => customer.Isdeleted == false);

        if(!string.IsNullOrEmpty(search)){
        query = query.Where(customer => customer.Customername.ToLower().Trim().Contains(search.ToLower().Trim()));
        }
        var a = query.ToList();
        if (!string.IsNullOrEmpty(filterBy) && startDate == null && endDate == null)
        {
            switch (filterBy)
            {
                case "Last 7 days":
                    query = query.Where(customer => customer.Createdat >= currentDate.AddDays(-7));
                    break;

                case "Last 30 days":
                    query = query.Where(customer => customer.Createdat >= currentDate.AddDays(-30));
                    break;

                case "Current Month":
                    query = query.Where(customer => customer.Createdat.HasValue &&
                                              customer.Createdat.Value.Month == currentDate.Month &&
                                              customer.Createdat.Value.Year == currentDate.Year);
                    break;
            }
        }

        if (startDate != null && endDate != null)
        {
            query = query.Where(customer => customer.Createdat >= startDate && customer.Createdat <= endDate);
        }

        if (sortBy == "name")
        {
            if (sortOrder == "desc")
            {
                query = query.OrderByDescending(customer => customer.Customername);
            }
            else
            {
                query = query.OrderBy(Customer => Customer.Customername);
            }
        }
        if (sortBy == "date")
        {
            if (sortOrder == "desc")
            {
                query = query.OrderByDescending(customer => customer.Createdat);
            }
            else
            {
                query = query.OrderBy(Customer => Customer.Createdat);
            }
        }
        if (sortBy == "totalOrders")
        {
            if (sortOrder == "desc")
            {
                query = query.OrderByDescending(customer => customer.Totalorders);
            }
            else
            {
                query = query.OrderBy(Customer => Customer.Totalorders);
            }
        }

        model.totalCustomers = query.ToList().Count();
        if(isExport != true)
        {
        query = query.Where(customer => customer.Isdeleted == false)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize);
        }
        model.pageNumber = pageNumber;
        model.pageSize = pageSize;
        model.sortBy = sortBy;
        model.sortOrder = sortOrder;
        model.customers = query.ToList();
        return model;

    }

    public CustomerViewModel getCustomerHistory(int customerid)
    {
        Customer customer = _context.Customers.FirstOrDefault(customer => customer.Customerid == customerid && customer.Isdeleted == false)!;
        CustomerViewModel model = new CustomerViewModel();
        model.customerName = customer.Customername;
        model.phoneNumber = customer.Phonenumber;
        List<Order> customerOrders = _context.Orders.Where(order => order.Customerid == customer.Customerid && order.IsDeleted == false).ToList();
        model.totalVisits = customerOrders.Count();
        decimal avg_order = 0;
        decimal max_order = 0;
        DateTime? coming_since = null;

        foreach (var order in customerOrders)
        {
            avg_order += order.Totalamount ?? 0;
            if (order.Totalamount > max_order)
            {
                max_order = order.Totalamount ?? 0;
                            }
            if (coming_since == null)
            {
                coming_since = order.Createdat;
            }
            else
            {
                if (coming_since > order.Createdat)
                {
                    coming_since = order.Createdat;
                }
            }
        }
        if(customerOrders.Count() > 0)
    {

        model.avg_order = avg_order / customerOrders.Count();
    }
    else{
        model.avg_order = 0;

    }
        model.max_order = max_order;
        model.comingAt = coming_since ?? DateTime.MinValue;

        List<OrderDetailModel> orderDetails = new List<OrderDetailModel>();
        foreach (var order in customerOrders)
        {
            OrderDetailModel orderDetail = new OrderDetailModel();
            orderDetail.orderDate = order.Createdat ?? DateTime.MinValue;
            orderDetail.orderType = "DineIn";
            orderDetail.paymentStatus = order.PaymentStatus!;
            orderDetail.noOfItems = _context.Ordermodifiers.Where(orderModifiers => orderModifiers.Orderid == order.Orderid && orderModifiers.Isdeleted == false).GroupBy(om => om.Itemid).Count();
            orderDetail.amount = order.Totalamount ?? 0;
            orderDetails.Add(orderDetail);
        }
        model.orderDetails = orderDetails;
        return model;
    }
}