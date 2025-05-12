using DAL.Data;

namespace DAL.interfaces
{
    public interface ICustomerRepository 
    {
        Customer createNewCustomer(CustomerModel customerModal);
        CustomerViewModel getAllCustomers(int pageSize,int pageNumber,string sortBy,string sortOrder,string? search,string filterBy,DateTime? startDate,DateTime? endDate,bool? isExport);
        CustomerViewModel getCustomerHistory(int customerid);
    }
}