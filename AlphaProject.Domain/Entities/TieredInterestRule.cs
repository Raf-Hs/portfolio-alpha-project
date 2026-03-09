namespace AlphaProject.Domain.Entities;

public class TieredInterestRule
{
    public int Id { get; set; }
    public int InstitutionId { get; set; }
    public decimal LimitAmount { get; set; }
    public decimal PrimaryRate { get; set; }
    public decimal FallbackRate { get; set; }
    public Institution Institution { get; set; } = null!;
}
