using System.Text;
using Infrastructure;
using Infrastructure.Persistence;
using Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FluentValidation;
using Application.Validators.Generic;
using Application.Validators.Assessment;
using WebAPI.ExceptionHandlers;
using MediatR;
using Application.Reports.Queries.GetDecommissionLastYear;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // Accept both PascalCase and camelCase
    });

// Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// DBContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 39))
    )
);

// Registrar Infrastructure (Repos, UoW, JwtProvider)
builder.Services.AddInfrastructure(builder.Configuration);

// Registro explícito de MediatR y repositorio de queries de reportes (por claridad, aunque AddInfrastructure ya lo registra)
builder.Services.AddMediatR(typeof(GetDecommissionLastYearQuery).Assembly);

// Registrar Application (Services + AutoMapper)
builder.Services.AddApplication();

// JWT Auth
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        )
    };
});

//builder.Services.AddAuthorization();

// CORS para React
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Register all validators defined in the Application.Validators assembly
builder.Services.AddValidatorsFromAssemblyContaining<UpdateAssessmentDtoValidator>();

// Register a fallback validator for any requested DTO validator, to avoid DI resolution failures
// when specific CreateXDto validators are not yet implemented.
builder.Services.AddSingleton(typeof(IValidator<>), typeof(Application.Validators.Generic.NoOpValidator<>));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(GetDecommissionLastYearQuery).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // ⭐ AGREGAR ESTO
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Migraciones automáticas
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    ctx.Database.Migrate();
}

app.Run();
