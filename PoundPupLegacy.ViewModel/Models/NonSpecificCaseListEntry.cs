namespace PoundPupLegacy.ViewModel.Models;

public record NonSpecificCaseListEntry : CaseListEntry
{
    public required string CaseType { get; init; }
}
