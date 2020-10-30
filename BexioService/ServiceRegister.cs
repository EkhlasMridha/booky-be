using BexioService.BexioHttp;
using BexioService.interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BexioService
{
    public static class ServiceRegister
    {
        public static void AddBexio(this IServiceCollection services)
        {
            services.AddScoped<HttpMiddleware>();
            services.AddHttpClient<IBexioAPI ,BexioAPI>().AddHttpMessageHandler<HttpMiddleware>();
        }
    }
}
