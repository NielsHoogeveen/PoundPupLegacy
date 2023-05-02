namespace PoundPupLegacy.ViewModel.Models;

public record DeportationCases : IPagedList<CaseListEntry>
{
    public required CaseListEntry[] Entries { get; init; }
    public required int NumberOfEntries { get; init; }

}
