using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // 🔹 Registro global de AutoMapper (escanea todos los Profiles del proyecto Application)
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
