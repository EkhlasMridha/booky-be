using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DataManagers.AuthManagers.interfaces;
using DataManagers.AuthManagers;
using DataManagers.Managers.interfaces;
using DataManagers.Managers;

namespace DataManagers
{
    public static class ServiceRegister
    {
        public static void AddDataManagers(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddDbContext<HotelDbContext>(options => 
                options.UseMySQL(Configuration.GetConnectionString("HotelDbContect"),
                    mySqlOptions =>
                    {
                        mySqlOptions.ExecutionStrategy(c => new MySqlRetryingExecutionStrategy(c));
                    }
               )

            );
            services.AddScoped<IAdminManager, AdminManager>();
            services.AddScoped<IRoomDataManager, RoomDataManager>();
            services.AddScoped<IBookingDataManager, BookingDataManager>();
            services.AddScoped<ISessionDataManager, SessionDataManager>();
        }
    }
}
