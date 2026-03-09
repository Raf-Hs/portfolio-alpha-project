namespace AlphaProject.Domain.Entities;

public class Institution
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<TieredInterestRule> InterestRules { get; set; } = new List<TieredInterestRule>();
}
