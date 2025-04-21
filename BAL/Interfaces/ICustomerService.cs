using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Data;
using Microsoft.AspNetCore.Mvc;

namespace BAL.Interfaces
{
    public interface ICustomerService
    {
        byte[] exportCustomerDetails(int pageSize,int pageNumber,string sortBy,string sortOrder,string search,string filterBy,DateTime? startDate,DateTime? endDate,bool? isExport);
        public CustomerViewModel getAllCustomers(int pageSize,int pageNumber,string sortBy,string sortOrder,string? search,string filterBy,DateTime? startDate,DateTime? endDate);
        CustomerViewModel getCustomerHistory(int customerid);
    }
}