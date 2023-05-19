namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BlogPostTeaser))]
public partial class BlogPostTeaserJsonContext : JsonSerializerContext { }

public sealed record BlogPostTeaser : AuthoredTeaserListEntryBase
{
}