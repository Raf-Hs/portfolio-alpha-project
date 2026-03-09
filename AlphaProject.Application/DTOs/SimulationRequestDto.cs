namespace AlphaProject.Application.DTOs;

public class SimulationRequestDto
{
    public int InstitutionId { get; set; }
    public decimal InvestmentAmount { get; set; }
    public int Months { get; set; }
}
