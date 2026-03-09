using AlphaProject.Application.Services;
using AlphaProject.Domain.Interfaces;
using AlphaProject.Domain.Services;
using AlphaProject.Domain.Strategies;
using AlphaProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. REGISTRO DE SERVICIOS (Dependency Injection) ---
builder.Services.AddControllers();

// Agregamos los servicios necesarios para generar el JSON de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AlphaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Tus servicios de dominio y aplicación
builder.Services.AddScoped<IProjectionStrategy, CompoundInterestStrategy>();
builder.Services.AddScoped<IProjectionAppService, ProjectionAppService>();

// Sofipo services
builder.Services.AddScoped<ISofipoYieldStrategy, SofipoYieldStrategy>();
builder.Services.AddScoped<ISofipoAppService, SofipoAppService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("AllowAll");

// --- 2. CONFIGURACIÓN DEL PIPELINE HTTP (Middlewares) ---
if (app.Environment.IsDevelopment())
{
    // Activamos Swagger solo en desarrollo
    app.UseSwagger();
    app.UseSwaggerUI();
}

// COMENTAMOS esta línea por el momento (MVP) para evitar el error de redirección HTTPS
// app.UseHttpsRedirection(); 

app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AlphaDbContext>();
    await SofipoSeeder.SeedAsync(dbContext);
}

app.Run();