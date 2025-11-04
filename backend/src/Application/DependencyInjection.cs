using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Registrar servicios de la capa de aplicación
            services.AddScoped<TechnicalDowntimeService>();
            
            // Aquí puedes agregar más servicios según los necesites
            // services.AddScoped<OtroServicio>();

            return services;
        }
    }
}
