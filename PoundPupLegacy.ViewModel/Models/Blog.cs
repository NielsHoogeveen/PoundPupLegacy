namespace PoundPupLegacy.ViewModel.Models;

public record Blog 
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required List<BlogPostTeaser> BlogPostTeasers { get; init; }
    public required int NumberOfEntries { get; init; }
}
