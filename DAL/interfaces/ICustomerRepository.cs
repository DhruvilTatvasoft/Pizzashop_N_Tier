

using DAL.Data;

namespace DAL.interfaces
{
    public interface ICustomerRepository 
    {
        CustomerViewModel getAllCustomers(int pageSize,int pageNumber,string sortBy,string sortOrder,string? search,string filterBy,DateTime? startDate,DateTime? endDate);
    }
}