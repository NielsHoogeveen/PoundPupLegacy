namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(ArticleList))]
public partial class ArticleListJsonContext : JsonSerializerContext { }

public sealed record ArticleList : PagedListBase<ArticleListEntry>
{
}
