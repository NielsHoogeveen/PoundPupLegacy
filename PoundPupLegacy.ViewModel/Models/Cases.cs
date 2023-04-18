namespace PoundPupLegacy.ViewModel.Models;

public record Cases 
{
    public required CaseListEntry[] CaseListEntries { get; init; }
    public required int NumberOfEntries { get; init; }

}
