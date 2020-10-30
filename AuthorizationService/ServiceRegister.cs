using AuthorizationService.CorsConfig;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AuthorizationService
{
    public static class ServiceRegister
    {
        public static void AddAuthorizationPolicy(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<CorsHosts>(Configuration.GetSection(nameof(CorsHosts)));
            services.AddSingleton<ICorsHosts>(mailSetting => mailSetting.GetRequiredService<IOptions<CorsHosts>>().Value);
            services.AddCors();
            services.AddSingleton<ICorsPolicyProvider, CorsPolicyProvider>();
        }
    }
}
