using Microsoft.Extensions.DependencyInjection;
using Application.Mappers;
using Application.Interfaces.Services;
using Application.Services;
using AutoMapper;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AssessmentMapper).Assembly);

            // SERVICIOS
            services.AddScoped<IAssessmentService, AssessmentService>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<IEquipmentDecommissionService, EquipmentDecommissionService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<ITechnicalService, TechnicalService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDirectorService, DirectorService>();
            services.AddScoped<IEquipmentService, EquipmentService>();
            services.AddScoped<IEquipmentTypeService, EquipmentTypeService>();
            services.AddScoped<IResponsibleService, ResponsibleService>();

            return services;
        }
    }
}