using System.Text.Json.Serialization;

namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BlogPostTeaser))]
public partial class BlogPostTeaserJsonContext : JsonSerializerContext { }

public sealed record BlogPostTeaser : AuthoredTeaserListEntry
{
    public required string Path { get; init; }

    public required string Title { get; init; }

    public required Authoring Authoring { get; init; }

    public required string Text { get; init; }

    public required bool HasBeenPublished { get; init; }

}