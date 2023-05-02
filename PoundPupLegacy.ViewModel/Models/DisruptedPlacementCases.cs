namespace PoundPupLegacy.ViewModel.Models;

public record DisruptedPlacementCases : IPagedList<CaseListEntry>
{
    public required CaseListEntry[] Entries { get; init; }
    public required int NumberOfEntries { get; init; }

}
