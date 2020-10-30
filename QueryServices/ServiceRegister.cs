using Microsoft.Extensions.DependencyInjection;
using QueryServices.Paging;
using System;
using System.Collections.Generic;
using System.Text;

namespace QueryServices
{
    public static class ServiceRegister
    {
        public static void AddQueryServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IPageHandler<>), typeof(PageHandler<>));
        }
    }
}
