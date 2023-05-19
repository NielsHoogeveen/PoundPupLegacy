namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(BlogPost))]
public partial class BlogPostJsonContext : JsonSerializerContext { }

public sealed record class BlogPost : SimpleTextNodeBase
{
}
