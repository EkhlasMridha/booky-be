using AuthorizationService.CorsConfig;
using Microsoft.AspNetCore.Builder;

namespace AuthorizationService
{
    public static class MiddlewareConfig
    {
        public static void UseCustomCors(this IApplicationBuilder app)
        {
            app.UseMiddleware<CorsPolicyMiddleware>();
        }
    }
}
