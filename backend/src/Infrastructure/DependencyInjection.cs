using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Application.Interfaces.Security;
using Infrastructure.Security;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // 🔹 Unit of Work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // 🔹 Repositorios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDirectorRepository, DirectorRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ITechnicalRepository, TechnicalRepository>();
            services.AddScoped<IResponsibleRepository, ResponsibleRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ISectionRepository, SectionRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            services.AddScoped<IEquipmentTypeRepository, EquipmentTypeRepository>();
            services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
            services.AddScoped<IAssessmentRepository, AssessmentRepository>();
            services.AddScoped<ITransferRepository, TransferRepository>();
            services.AddScoped<IDestinyTypeRepository, DestinyTypeRepository>();
            services.AddScoped<IEquipmentDecommissionRepository, EquipmentDecommissionRepository>();

            // 🔹 JwtProvider = infraestructura técnica → debe estar aquí
            services.AddSingleton<IJwtProvider, JwtProvider>();

            return services;
        }
    }
}
