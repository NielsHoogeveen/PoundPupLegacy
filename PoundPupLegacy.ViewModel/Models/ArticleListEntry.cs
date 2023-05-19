namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ArticleListEntry))]
public partial class ArticleListEntryJsonContext : JsonSerializerContext { }

public sealed record ArticleListEntry : TaggedTeaserListEntryBase
{
}
