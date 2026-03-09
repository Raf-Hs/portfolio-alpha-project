using AlphaProject.Application.Services;
using AlphaProject.Domain.Interfaces;
using AlphaProject.Domain.Services;

var builder = WebApplication.CreateBuilder(args);

// --- 1. REGISTRO DE SERVICIOS (Dependency Injection) ---
builder.Services.AddControllers();

// Agregamos los servicios necesarios para generar el JSON de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Tus servicios de dominio y aplicación
builder.Services.AddScoped<IProjectionStrategy, CompoundInterestStrategy>();
builder.Services.AddScoped<IProjectionAppService, ProjectionAppService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("https://portfolio-alpha-project.vercel.app") // El puerto de tu frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAngular");

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

app.Run();