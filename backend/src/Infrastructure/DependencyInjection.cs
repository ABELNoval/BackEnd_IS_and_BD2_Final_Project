using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Application.Interfaces.Security;
using Infrastructure.Security;
using Application.Interfaces.Services;
using Infrastructure.Reports;
using Application.Reports.Generators;
using Infrastructure.Reports.Generators;
using Application.Interfaces.Repositories;

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
            services.AddScoped<ITransferRequestRepository, TransferRequestRepository>();
            services.AddScoped<IDestinyTypeRepository, DestinyTypeRepository>();
            services.AddScoped<IEquipmentDecommissionRepository, EquipmentDecommissionRepository>();

            // 🔹 JwtProvider = infraestructura técnica → debe estar aquí
            services.AddSingleton<IJwtProvider, JwtProvider>();



            // Register all report generators (explicit, for constructor injection)
            services.AddScoped<PdfReportGenerator>();
            services.AddScoped<ExcelReportGenerator>();
            services.AddScoped<WordReportGenerator>();

            // Register the factory and the service
            services.AddScoped<IReportGeneratorFactory, ReportGeneratorFactory>();
            services.AddScoped<IReportService, ReportService>();

            services.AddScoped<IReportQueriesRepository, ReportQueriesRepository>();

            return services;
        }
    }
}
