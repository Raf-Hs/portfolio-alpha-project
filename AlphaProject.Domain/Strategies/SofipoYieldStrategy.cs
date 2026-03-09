using AlphaProject.Domain.Entities;

namespace AlphaProject.Domain.Strategies;

public class SofipoYieldStrategy : ISofipoYieldStrategy
{
    public decimal CalculateMonthlyYield(decimal investmentAmount, TieredInterestRule rule)
    {
        if (investmentAmount <= rule.LimitAmount)
        {
            return investmentAmount * (rule.PrimaryRate / 100 / 12);
        }

        var primaryYield = rule.LimitAmount * (rule.PrimaryRate / 100 / 12);
        var excessAmount = investmentAmount - rule.LimitAmount;
        var fallbackYield = excessAmount * (rule.FallbackRate / 100 / 12);

        return primaryYield + fallbackYield;
    }
}
