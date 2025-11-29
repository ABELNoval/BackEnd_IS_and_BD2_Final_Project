using System.Text;
using Infrastructure;
using Infrastructure.Persistence;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Application.Interfaces.Services;
using Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Application.Interfaces.Security;


var builder = WebApplication.CreateBuilder(args) ;

// ===========================================
// 1️⃣  Agregar Controllers
// ===========================================
builder.Services.AddControllers();

// ===========================================
// 2️⃣  AutoMapper (mira si ya tienes Assembly scanning)
// ===========================================
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ===========================================
// 3️⃣  Registrar AppDbContext con MySQL
// ===========================================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 39))
    )
);

// ===========================================
// 4️⃣  Registrar capa Infrastructure (repos, UoW, etc.)
// ===========================================
builder.Services.AddInfrastructure(builder.Configuration);

// ===========================================
// 5️⃣  Registrar servicios de autenticación y JWT
// ===========================================
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IJwtProvider, JwtProvider>();

// ===========================================
// 6️⃣  Configurar Authentication JWT
// ===========================================
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

// ===========================================
// 7️⃣  Agregar autorización
// ===========================================
builder.Services.AddAuthorization();

// ===========================================
// 8️⃣  CORS para permitir llamadas desde React
// ===========================================
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

var app = builder.Build();

// ===========================================
// 9️⃣  Middleware pipeline
// ===========================================
app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthentication();   // 🔥 OBLIGATORIO para JWT
app.UseAuthorization();

app.MapControllers();

// ===========================================
// 🔟  Migrar BD automáticamente
// ===========================================
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    ctx.Database.Migrate();
}

app.Run();
