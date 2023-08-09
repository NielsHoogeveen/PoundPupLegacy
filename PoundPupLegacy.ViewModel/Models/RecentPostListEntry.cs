namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(RecentPosts))]
public partial class RecentPostsJsonContext : JsonSerializerContext { }

public sealed record RecentPosts : PagedListBase<RecentPostListEntry>
{
}
[JsonSerializable(typeof(RecentPostListEntry))]
public partial class RecentPostListEntryJsonContext : JsonSerializerContext { }

public sealed record RecentPostListEntry: ListEntry
{
    public required string Title { get; init; }
    public required string Path { get; init; }
    public required string Publisher { get; init; }
    public required string NodeType { get; init; }
    public required DateTime DateTime { get; init; }

}
