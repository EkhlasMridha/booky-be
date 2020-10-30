using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AuthorizationService.CorsConfig
{
    class CorsPolicyProvider : ICorsPolicyProvider
    {
        private ICorsHosts _corsHosts;
        public CorsPolicyProvider(ICorsHosts client)
        {
            _corsHosts = client;
        }

        public Task<CorsPolicy> GetPolicyAsync(HttpContext context, string policyName)
        {
            string result =  _corsHosts.AllowedHost;


            var allowedHosts = result.Split(",");

            var policy = new CorsPolicyBuilder()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins(allowedHosts)
                        .Build();

            return Task.FromResult(policy);
        }
    }
}
