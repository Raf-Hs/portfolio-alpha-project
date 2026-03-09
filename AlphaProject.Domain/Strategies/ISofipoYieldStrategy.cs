using AlphaProject.Domain.Entities;

namespace AlphaProject.Domain.Strategies;

public interface ISofipoYieldStrategy
{
    decimal CalculateMonthlyYield(decimal investmentAmount, TieredInterestRule rule);
}
