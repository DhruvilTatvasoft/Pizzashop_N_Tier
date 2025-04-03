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

        public int getAllCustomerCount()
        {
            return _customerRepository.getAllCustomerCount();
        }

        public List<Customer> getAllCustomers(int pageSize,int pageNumber,string sortBy,string sortOrder,string? search)
        {
            CustomerViewModel model = new CustomerViewModel();
            List<Customer> allCustomers = _customerRepository.getAllCustomers(pageSize,pageNumber,sortBy,sortOrder,search);
            return allCustomers;

        }
    }
}