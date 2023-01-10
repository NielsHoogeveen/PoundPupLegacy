namespace PoundPupLegacy.ViewModel;

public record struct Blog
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int NumberOfEntries { get; set; }
    public List<BlogPostTeaser> BlogPostTeasers { get; set; }

    public int PageNumber { get; set; }

    public int NumberOfPages { get; set; }
}
