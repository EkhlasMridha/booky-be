using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Models.Dtos;

namespace BexioService.interfaces
{
    public interface IBexioAPI
    {
        public Task<IEnumerable<BexioCustomerDto>> GetContactsAsync();
        public Task<BexioCustomer> CreateCustomer(BexioCustomer bexioCustomer);
        public Task<BexioCustomerDto> GetCustomerById(int id);
        public Task DeleteCustomerById(int id);
        public Task<BexioCustomerDto> EditCustomer(int id, BexioCustomerDto bexioCustomer);
        public Task<IEnumerable<BexioCustomerDto>> SearchCustomer(CustomerSearch[] customerSearch);
        public Task<IEnumerable<BexioCountry>> GetBexioCountry();
        public Task<bool> CheckValidity(CustomerSearch[] customerSearche);
    }
}
