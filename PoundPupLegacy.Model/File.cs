namespace PoundPupLegacy.CreateModel;

public sealed record File : Identifiable
{
    public required int? Id { get; set; }

    public required String Path { get; init; }

    public required String Name { get; init; }

    public required String MimeType { get; init; }

    public required int Size { get; init; }
    public required List<TenantFile> TenantFiles { get; init; }
}
