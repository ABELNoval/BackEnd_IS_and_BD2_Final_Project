using Microsoft.Extensions.DependencyInjection;
using Application.Mappers;
using Application.Interfaces.Services;
using Application.Services;
using AutoMapper;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
                    // 🔹 Validadores personalizados (FluentValidation)
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Technical.CreateTechnicalDto>,
                        Application.Validators.Technical.CreateTechnicalDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Technical.UpdateTechnicalDto>,
                        Application.Validators.Technical.UpdateTechnicalDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.EquipmentType.CreateEquipmentTypeDto>,
                        Application.Validators.EquipmentType.CreateEquipmentTypeDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.EquipmentType.UpdateEquipmentTypeDto>,
                        Application.Validators.EquipmentType.UpdateEquipmentTypeDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Section.CreateSectionDto>,
                        Application.Validators.Section.CreateSectionDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Section.UpdateSectionDto>,
                        Application.Validators.Section.UpdateSectionDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Responsible.CreateResponsibleDto>,
                        Application.Validators.Responsible.CreateResponsibleDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Responsible.UpdateResponsibleDto>,
                        Application.Validators.Responsible.UpdateResponsibleDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.User.CreateUserDto>,
                        Application.Validators.User.CreateUserDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.User.UpdateUserDto>,
                        Application.Validators.User.UpdateUserDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Transfer.CreateTransferDto>,
                        Application.Validators.Transfer.CreateTransferDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Transfer.UpdateTransferDto>,
                        Application.Validators.Transfer.UpdateTransferDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.EquipmentDecommission.CreateEquipmentDecommissionDto>,
                        Application.Validators.EquipmentDecommission.CreateEquipmentDecommissionDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.EquipmentDecommission.UpdateEquipmentDecommissionDto>,
                        Application.Validators.EquipmentDecommission.UpdateEquipmentDecommissionDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Maintenance.CreateMaintenanceDto>,
                        Application.Validators.Maintenance.CreateMaintenanceDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Maintenance.UpdateMaintenanceDto>,
                        Application.Validators.Maintenance.UpdateMaintenanceDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Employee.CreateEmployeeDto>,
                        Application.Validators.Employee.CreateEmployeeDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Employee.UpdateEmployeeDto>,
                        Application.Validators.Employee.UpdateEmployeeDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Equipment.CreateEquipmentDto>,
                        Application.Validators.Equipment.CreateEquipmentDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Equipment.UpdateEquipmentDto>,
                        Application.Validators.Equipment.UpdateEquipmentDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Department.CreateDepartmentDto>,
                        Application.Validators.Department.CreateDepartmentDtoValidator>();
                    services.AddScoped<FluentValidation.IValidator<Application.DTOs.Department.UpdateDepartmentDto>,
                        Application.Validators.Department.UpdateDepartmentDtoValidator>();
        
            services.AddAutoMapper(typeof(AssessmentMapper).Assembly);
          
            // 🔹 Registrar AutoMapper
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            // 🔹 Registrar servicios de aplicación
            services.AddScoped<IEquipmentService, EquipmentService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ISectionService, SectionService>();
            services.AddScoped<IEquipmentTypeService, EquipmentTypeService>();
            services.AddScoped<IResponsibleService, ResponsibleService>();
            services.AddScoped<IMaintenanceService, MaintenanceService>();
            services.AddScoped<IDirectorService, DirectorService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ITechnicalService, TechnicalService>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<IEquipmentDecommissionService, EquipmentDecommissionService>();
            services.AddScoped<IAssessmentService, AssessmentService>();
            // services.AddScoped<IUserService, UserService>();

            // AuthService - JWT authentication service
            services.AddScoped<IAuthService, AuthService>();

            // 🔹 Validadores EquipmentDecommission (con dependencias)
            services.AddScoped<FluentValidation.IValidator<Application.DTOs.EquipmentDecommission.CreateEquipmentDecommissionDto>,
                Application.Validators.EquipmentDecommission.CreateEquipmentDecommissionDtoValidator>();
            services.AddScoped<FluentValidation.IValidator<Application.DTOs.EquipmentDecommission.UpdateEquipmentDecommissionDto>,
                Application.Validators.EquipmentDecommission.UpdateEquipmentDecommissionDtoValidator>();

            return services;
        }
    }
}