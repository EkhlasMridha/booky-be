using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BexioService.interfaces;
using DataManagers.Managers.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Entities;
using QueryServices;
using QueryServices.Paging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelManagement.Controllers
{
    [Route("v1/customer")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private IMapper _mapper;
        private IBexioAPI _bexioAPI;
        public CustomerController(IBexioAPI bexioAPI,IMapper mapper)
        {
            _mapper = mapper;
            _bexioAPI = bexioAPI;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery] QueryParameters queryParameters)
        {
            var data = await _bexioAPI.GetContactsAsync();

            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBexioContact([FromBody] BexioCustomer bexioCustomer)
        {
            var customer = await _bexioAPI.CreateCustomer(bexioCustomer);

            return Ok(customer);
        }

        [HttpPost("search")]
        public async Task<IActionResult> SearchBexioContact([FromBody] CustomerSearch[] customerSearch )
        {
            var data = await _bexioAPI.SearchCustomer(customerSearch);

            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerByID(int id)
        {
            var customer = await _bexioAPI.GetCustomerById(id);

            return Ok(customer);
        }

        [HttpGet("country")]
        public async Task<IActionResult> GetAvailCountryAsync()
        {
            var result = await _bexioAPI.GetBexioCountry();

            return Ok(result);
        } 

        [HttpGet("checkemail/{email}")]
        public async Task<IActionResult> CheckEmail(string email)
        {
            CustomerSearch[] searchList = new CustomerSearch[1];
            searchList[0] = new CustomerSearch
            {
                field = "mail",
                value = email,
                criteria = "="
            };

            var isExist = await _bexioAPI.CheckValidity(searchList);
            return Ok(isExist);
        }

        [HttpGet("checkphone/{phone}")]
        public async Task<IActionResult> CheckPhoneNumber(string phone)
        {
            CustomerSearch[] searchList = new CustomerSearch[1];
            searchList[0] = new CustomerSearch
            {
                field = "phone_mobile",
                value = phone,
                criteria = "="
            };
            var isExist = await _bexioAPI.CheckValidity(searchList);
            return Ok(isExist);
        }

        // PUT api/<CustomerController>/5
        [HttpPut]
        public async Task<IActionResult> UpdateCustomer([FromBody] BexioCustomerDto bexioCustomerDto)
        {
            var update = await _bexioAPI.EditCustomer(bexioCustomerDto.id,bexioCustomerDto);
            return Ok(update);
        }
    }
}
