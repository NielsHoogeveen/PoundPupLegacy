namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Blog))]
public partial class BlogJsonContext : JsonSerializerContext { }

public sealed record Blog : PagedListBase<BlogPostTeaser>
{
    public required int Id { get; init; }
    public required string Name { get; init; }
}
