using BexioService.interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Models.Entities;
using Models.Dtos;
using System.Linq;

namespace BexioService.BexioHttp
{
    public class BexioAPI : IBexioAPI
    {
        private HttpClient _client;
        public BexioAPI(HttpClient client)
        {
            client.BaseAddress = new Uri("https://api.bexio.com/2.0/");
            _client = client;
        }

        public async Task<bool> CheckValidity(CustomerSearch[] customerSearch)
        {
            var _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                AllowTrailingCommas = true
            };

            var customerJson = JsonSerializer.Serialize(customerSearch);

            using var response = await _client.PostAsync("contact/search", new StringContent(customerJson, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var data = await JsonSerializer.DeserializeAsync<ICollection<BexioCustomerDto>>(responseStream);

            if(data.Count == 0)
            {
                return false;
            }else
            {
                return true;
            }
        }

        public async Task<BexioCustomer> CreateCustomer(BexioCustomer bexioCustomer)
        {
            var _jsonSerializerOptions = new JsonSerializerOptions { 
                WriteIndented = true,
                AllowTrailingCommas = true
            };

            var customerJson =JsonSerializer.Serialize(bexioCustomer);

            using var customer = await _client.PostAsync("contact", new StringContent(customerJson,Encoding.UTF8,"application/json"));

            customer.EnsureSuccessStatusCode();
            return bexioCustomer;
        }

        public Task DeleteCustomerById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<BexioCustomerDto> EditCustomer(int id, BexioCustomerDto bexioCustomer)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = "https";
            uriBuilder.Host = "api.bexio.com";
            uriBuilder.Path = "2.0/contact/" + id;
            Uri url = uriBuilder.Uri;

            var customerJson = JsonSerializer.Serialize(bexioCustomer);

            using var response = await _client.PostAsync(url, new StringContent(customerJson, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<BexioCustomerDto>(responseStream);
        }

        public async Task<IEnumerable<BexioCountry>> GetBexioCountry()
        {
            var response = await _client.GetAsync("country");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<IEnumerable<BexioCountry>>(responseStream);
        }

        public async Task<IEnumerable<BexioCustomerDto>> GetContactsAsync()
        {
            var response = await _client.GetAsync("contact");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<IEnumerable<BexioCustomerDto>>(responseStream);
        }

        public async Task<BexioCustomerDto> GetCustomerById(int id)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = "https";
            uriBuilder.Host = "api.bexio.com";
            uriBuilder.Path = "2.0/contact/" + id;

            Uri url = uriBuilder.Uri;
            var response = await _client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            using var responseStream = await response.Content.ReadAsStreamAsync();

            return await JsonSerializer.DeserializeAsync<BexioCustomerDto>(responseStream);
        }

        public async Task<IEnumerable<BexioCustomerDto>> SearchCustomer(CustomerSearch[] customerSearch)
        {
            var _jsonSerializerOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
                AllowTrailingCommas = true
            };

            var customerJson = JsonSerializer.Serialize(customerSearch);

            using var response = await _client.PostAsync("contact/search", new StringContent(customerJson, Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            using var customerData = await response.Content.ReadAsStreamAsync();

            var data = await JsonSerializer.DeserializeAsync<IEnumerable<BexioCustomerDto>>(customerData);

            return data;
        }
    }
}
