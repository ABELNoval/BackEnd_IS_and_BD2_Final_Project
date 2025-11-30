using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Interfaces.Services;
using Application.Services;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // 🔹 Registrar AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // 🔹 Registrar servicios de aplicación
            services.AddScoped<IEquipmentService, EquipmentService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IEquipmentTypeService, EquipmentTypeService>();
            services.AddScoped<IResponsibleService, ResponsibleService>();
            // services.AddScoped<IUserService, UserService>();

            // AuthService va aquí porque es lógicamente un servicio de aplicación
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}
