namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(Document))]
public partial class DocumentJsonContext : JsonSerializerContext { }

public sealed record Document : SimpleTextNodeBase
{
    public required FuzzyDate? Published { get; init; }
    public BasicLink? DocumentType { get; init; }
    public string? SourceUrl { get; init; }
    public string? SourceUrlHost => MakeUri(SourceUrl)?.Host;

    private Uri? MakeUri(string? str)
    {
        if (str is null)
            return null;
        if (Uri.TryCreate(str, UriKind.Absolute, out var uri)) {
            return uri;
        }
        else if (!str.Contains(":")) {
            if (Uri.TryCreate($"http://{str}", UriKind.Absolute, out var uri2)) {
                return uri2;
            }
            return null;
        }
        return null;
    }
}
