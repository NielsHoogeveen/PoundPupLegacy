namespace PoundPupLegacy.ViewModel.Models;

public record Blog : IPagedList<BlogPostTeaser>
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required BlogPostTeaser[] Entries { get; init; }
    public required int NumberOfEntries { get; init; }
}
