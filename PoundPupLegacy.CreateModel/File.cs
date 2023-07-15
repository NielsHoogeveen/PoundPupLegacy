namespace PoundPupLegacy.DomainModel;

public sealed record File : PossiblyIdentifiable
{
    public required Identification.Possible Identification { get; init; }
    public required string Path { get; init; }
    public required string Name { get; init; }
    public required string MimeType { get; init; }
    public required int Size { get; init; }
    public required List<TenantFile> TenantFiles { get; init; }
}
