namespace PoundPupLegacy.ViewModel;

public record OrganizationListEntry: ListEntry
{
    public string Path { get; set; }
    public string Title { get; set; }
}
