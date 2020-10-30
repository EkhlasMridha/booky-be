using System.Net.Http;
using DataManagers;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace BexioService
{
    public class HttpMiddleware : DelegatingHandler
    {
        private HotelDbContext _hotelDbContext;

        public HttpMiddleware(HotelDbContext hotelDbContext)
        {
            _hotelDbContext = hotelDbContext;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
        {
            //var claim = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
            //string userId = claim.FindFirst(ZeroClaims.UserId)?.Value;

            //int id = Int32.Parse(userId);
            var user = await _hotelDbContext.Admin.FirstOrDefaultAsync(a => a.UserId == 1);

            if (user.BexioToken == null)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent("NO_BEXIO_TOKEN_FOUND")
                };
            }

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", "Bearer " + user.BexioToken);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
