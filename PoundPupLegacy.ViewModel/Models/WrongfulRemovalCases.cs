namespace PoundPupLegacy.ViewModel.Models;

public record WrongfulRemovalCases : IPagedList<CaseListEntry>
{
    public required CaseListEntry[] Entries { get; init; }
    public required int NumberOfEntries { get; init; }

}
