namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Documents))]
public partial class DocumentsJsonContext : JsonSerializerContext { }

public sealed record Documents : TermedListBase<ArticleList, ArticleListEntry>
{
}
