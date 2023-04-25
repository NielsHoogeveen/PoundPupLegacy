namespace PoundPupLegacy.CreateModel;

public interface Country : PoliticalEntity
{
    int HagueStatusId { get; }
    string? ResidencyRequirements { get; }
    string? AgeRequirements { get; }
    string? MarriageRequirements { get; }
    string? IncomeRequirements { get; }
    string? HealthRequirements { get; }
    string? OtherRequirements { get; }
    int? VocabularyIdSubdivisions { get; }

}
