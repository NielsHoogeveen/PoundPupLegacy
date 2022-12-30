namespace PoundPupLegacy.Model;

public interface Country : PoliticalEntity
{
    public int HagueStatusId { get; }
    public string? ResidencyRequirements { get; }
    public string? AgeRequirements { get; }
    public string? MarriageRequirements { get; }
    public string? IncomeRequirements { get; }
    public string? HealthRequirements { get; }
    public string? OtherRequirements { get; }
}
