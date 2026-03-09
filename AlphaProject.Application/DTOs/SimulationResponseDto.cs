namespace AlphaProject.Application.DTOs;

public class MonthlyBreakdownDto
{
    public int Month { get; set; }
    public decimal StartingBalance { get; set; }
    public decimal Yield { get; set; }
    public decimal EndingBalance { get; set; }
}

public class SimulationResponseDto
{
    public List<MonthlyBreakdownDto> MonthlyBreakdowns { get; set; } = new();
    public decimal TotalEarned { get; set; }
}
