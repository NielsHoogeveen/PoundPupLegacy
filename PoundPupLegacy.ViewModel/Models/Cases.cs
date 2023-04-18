namespace PoundPupLegacy.ViewModel.Models;

public record Cases: IPagedList<CaseListEntry>
{
    public required CaseListEntry[] Entries { get; init; }
    public required int NumberOfEntries { get; init; }

}
