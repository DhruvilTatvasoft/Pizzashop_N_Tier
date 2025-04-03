

using DAL.Data;

namespace DAL.interfaces
{
    public interface ICustomerRepository 
    {
        int getAllCustomerCount();
        List<Customer> getAllCustomers(int pageSize,int pageNumber,string sortBy,string sortOrder,string? search);
    }
}