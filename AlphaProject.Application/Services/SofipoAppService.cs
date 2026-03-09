using AlphaProject.Application.DTOs;
using AlphaProject.Application.Services;
using AlphaProject.Domain.Strategies;
using AlphaProject.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AlphaProject.Application.Services;

public class SofipoAppService : ISofipoAppService
{
    private readonly AlphaDbContext _dbContext;
    private readonly ISofipoYieldStrategy _yieldStrategy;

    public SofipoAppService(AlphaDbContext dbContext, ISofipoYieldStrategy yieldStrategy)
    {
        _dbContext = dbContext;
        _yieldStrategy = yieldStrategy;
    }

    public async Task<IEnumerable<SofipoItemDto>> GetAvailableSofiposAsync()
    {
        return await _dbContext.Institutions
            .Select(i => new SofipoItemDto
            {
                Id = i.Id,
                Name = i.Name
            })
            .ToListAsync();
    }

    public async Task<SimulationResponseDto> CalculateProjectionAsync(SimulationRequestDto request)
    {
        var institution = await _dbContext.Institutions
            .Include(i => i.InterestRules)
            .FirstOrDefaultAsync(i => i.Id == request.InstitutionId);

        if (institution == null)
        {
            throw new ArgumentException("Institución no encontrada.");
        }

        var rule = institution.InterestRules.FirstOrDefault();
        if (rule == null)
        {
            throw new ArgumentException("No hay reglas de interés para esta institución.");
        }

        var response = new SimulationResponseDto();
        var currentBalance = request.InvestmentAmount;
        decimal totalEarned = 0;

        for (int month = 1; month <= request.Months; month++)
        {
            var yieldAmount = _yieldStrategy.CalculateMonthlyYield(currentBalance, rule);
            totalEarned += yieldAmount;
            var endingBalance = currentBalance + yieldAmount;

            response.MonthlyBreakdowns.Add(new MonthlyBreakdownDto
            {
                Month = month,
                StartingBalance = currentBalance,
                Yield = yieldAmount,
                EndingBalance = endingBalance
            });

            currentBalance = endingBalance;
        }

        response.TotalEarned = totalEarned;
        return response;
    }
}
