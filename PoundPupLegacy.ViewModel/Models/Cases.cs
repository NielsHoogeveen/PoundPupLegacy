namespace PoundPupLegacy.ViewModel.Models;

public record Cases : IPagedList<NonSpecificCaseListEntry>
{
    public required CaseTypeListEntry[] CaseTypes { get; init; }
    public required NonSpecificCaseListEntry[] Entries { get; init; }
    public required int NumberOfEntries { get; init; }

}
