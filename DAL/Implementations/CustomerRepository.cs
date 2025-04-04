using DAL.Data;
using DAL.interfaces;

public class CustomerRepository : ICustomerRepository
{
    public PizzashopCContext _context;

    public CustomerRepository(PizzashopCContext context)
    {
        _context = context;
    }


    public CustomerViewModel getAllCustomers(int pageSize, int pageNumber, string sortBy, string sortOrder, string? search, string filterBy,DateTime? startDate, DateTime? endDate)
    {
        CustomerViewModel model = new CustomerViewModel();

        List<Customer> allCustomers = _context.Customers.Where(customer => customer.Isdeleted == false).ToList();
        DateTime currentDate = DateTime.Now;

        foreach (var customer in allCustomers)
        {
            if (customer.Totalorders == null)
            {
                customer.Totalorders = _context.Orders.Where(order => order.Customerid == customer.Customerid && order.IsDeleted == false).Count();
                _context.Customers.Update(customer);
                _context.SaveChanges();
            }
        }
        IQueryable<Customer> query;
        query = _context.Customers.Where(customer => customer.Isdeleted == false);
        query = query.Where(customer => search == "" || customer.Customername.ToLower().Trim().Contains(search.ToLower().Trim()));
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
        query = query.Where(customer => customer.Isdeleted == false)
                              .Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize);

        model.pageNumber = pageNumber;
        model.pageSize = pageSize;
        model.sortBy = sortBy;
        model.sortOrder = sortOrder;
        model.customers = query.ToList();
        return model;

    }
}