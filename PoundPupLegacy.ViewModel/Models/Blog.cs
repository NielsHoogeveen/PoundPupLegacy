namespace PoundPupLegacy.ViewModel.Models;

public record Blog : PagedList
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required List<BlogPostTeaser> BlogPostTeasers { get; init; }
    public int NumberOfEntries { get; set; }
    public int PageNumber { get; set; }
    public int NumberOfPages { get; set; }
    public string QueryString { get; set; } = "";
    public string Path => $"blog/{Id}";
}
