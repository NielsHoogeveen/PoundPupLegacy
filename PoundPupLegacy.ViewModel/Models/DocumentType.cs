namespace PoundPupLegacy.ViewModel.Models;

[JsonSerializable(typeof(DocumentType))]
public partial class DocumentTypeJsonContext : JsonSerializerContext { }

public sealed record DocumentType: NameableBase
{
}
