using Microsoft.Extensions.DependencyInjection;
using PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Model;
using PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Repository;

namespace PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Infrastructure
{
    public class ServiceRegister
    {
        public static void Register(IServiceCollection services, string connectionString)
        {
            services.AddScoped<IRepository<Person>, EfRepository<Person>>();
        }
    }
}
