// AlphaProject.Application/Services/ProjectionAppService.cs
using AlphaProject.Application.DTOs;
using AlphaProject.Application.Services;
using AlphaProject.Domain.Entities;
using AlphaProject.Domain.Interfaces;

namespace AlphaProject.Application.Services
{
    // Usamos Primary Constructors de .NET 8 para inyectar dependencias de forma limpia
    public class ProjectionAppService(IProjectionStrategy projectionStrategy) : IProjectionAppService
    {
        public async Task<IEnumerable<ProjectionRowDto>> GenerateProjectionAsync(CalculateProjectionRequest request)
        {
            // 1. Mapeo: DTO -> Domain Entity
            var parameters = new ProjectionParameters(
                request.InitialCapital, 
                request.AnnualInterestRate, 
                request.TermInMonths
            );

            // 2. Ejecución de la regla de negocio (Core)
            var domainRows = await projectionStrategy.CalculateAsync(parameters);

            // 3. Mapeo: Domain Entity -> DTO (Usamos LINQ optimizado)
            return domainRows.Select(row => new ProjectionRowDto(
                row.Period,
                row.StartingBalance,
                row.InterestEarned,
                row.EndingBalance
            ));
        }
    }
}