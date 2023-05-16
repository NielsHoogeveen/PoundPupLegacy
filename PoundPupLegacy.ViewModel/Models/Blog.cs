namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Blog))]
public partial class BlogJsonContext : JsonSerializerContext { }

public sealed record Blog : IPagedList<BlogPostTeaser>
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required BlogPostTeaser[] Entries { get; init; }
    public required int NumberOfEntries { get; init; }
}
