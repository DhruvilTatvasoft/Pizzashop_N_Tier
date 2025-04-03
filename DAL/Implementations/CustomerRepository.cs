using DAL.Data;
using DAL.interfaces;

public class CustomerRepository : ICustomerRepository
{
    public PizzashopCContext _context;

    public CustomerRepository(PizzashopCContext context)
    {
        _context = context;
    }

    public int getAllCustomerCount()
    {
        return _context.Customers.Where(customer => customer.Isdeleted == false).ToList().Count();
    }

    public List<Customer> getAllCustomers(int pageSize, int pageNumber, string sortBy, string sortOrder, string? search)
    {


        List<Customer> allCustomers = _context.Customers.Where(customer => customer.Isdeleted == false).ToList();


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
        if (search != "")
        {
            // tax.Taxname.ToLower().Trim().Contains(search.ToLower().Trim())
            query = _context.Customers.Where(customer => customer.Customername.ToLower().Trim().Contains(search.ToLower().Trim())&& customer.Isdeleted == false);
        }
        else
        {
            query = _context.Customers.Where(customer => customer.Isdeleted == false)
                                  .Skip((pageNumber - 1) * pageSize)
                                  .Take(pageSize);
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
        return query.ToList();
    }
}