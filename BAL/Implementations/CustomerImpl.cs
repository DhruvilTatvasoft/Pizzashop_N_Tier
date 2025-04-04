using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BAL.Interfaces;
using DAL.Data;
using DAL.interfaces;

namespace BAL.Implementations
{
    public class CustomerImpl : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerImpl(ICustomerRepository customerRepository){
            _customerRepository = customerRepository;
        }
        


        public CustomerViewModel getAllCustomers(int pageSize, int pageNumber, string sortBy, string sortOrder, string? search, string filterBy, DateTime? startDate, DateTime? endDate)
        {
           return _customerRepository.getAllCustomers(pageSize,pageNumber,sortBy,sortOrder,search,filterBy,startDate,endDate);
      
        }
    }
}