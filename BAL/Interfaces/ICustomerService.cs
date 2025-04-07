using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;

namespace BAL.Interfaces
{
    public interface ICustomerService
    {
        void exportCustomerDetails(int pageSize,int pageNumber,string sortBy,string sortOrder,string search,string filterBy,DateTime? startDate,DateTime? endDate,bool? isExport);
        public CustomerViewModel getAllCustomers(int pageSize,int pageNumber,string sortBy,string sortOrder,string? search,string filterBy,DateTime? startDate,DateTime? endDate);
        CustomerViewModel getCustomerHistory(int customerid);
    }
}