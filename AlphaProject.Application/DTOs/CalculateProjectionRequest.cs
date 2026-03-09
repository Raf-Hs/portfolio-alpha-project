// AlphaProject.Application/DTOs/CalculateProjectionRequest.cs
namespace AlphaProject.Application.DTOs
{
    // Usamos 'record' para garantizar inmutabilidad en los datos de entrada
    public record CalculateProjectionRequest(
        decimal InitialCapital, 
        decimal AnnualInterestRate, 
        int TermInMonths
    );
}