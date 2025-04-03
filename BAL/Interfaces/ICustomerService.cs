using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;

namespace BAL.Interfaces
{
    public interface ICustomerService
    {
        int getAllCustomerCount();
        public List<Customer> getAllCustomers(int pageSize,int pageNumber,string sortBy,string sortOrder,string? search);
    }
}