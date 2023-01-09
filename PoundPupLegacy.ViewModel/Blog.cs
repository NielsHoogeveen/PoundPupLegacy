namespace PoundPupLegacy.ViewModel;

public record struct Blog
{
    public string Name { get; set; }
    public int NumberOfEntries { get; set; }
    public List<BlogPostTeaser> BlogPostTeasers { get; set; }
}
