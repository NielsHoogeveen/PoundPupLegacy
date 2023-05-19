namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Page))]
public partial class PageJsonContext : JsonSerializerContext { }

public sealed record class Page : SimpleTextNodeBase
{
}
