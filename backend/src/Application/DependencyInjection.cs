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

            services.AddScoped<IAssessmentService, AssessmentService>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<IEquipmentDecommissionService, EquipmentDecommissionService>();

            return services;
        }
    }
}
