namespace PoundPupLegacy.CreateModel;

public sealed record File : EventuallyIdentifiable
{
    public required Identification.IdentificationForCreate IdentificationForCreate { get; init; }
    public Identification Identification => IdentificationForCreate;
    public required string Path { get; init; }
    public required string Name { get; init; }
    public required string MimeType { get; init; }
    public required int Size { get; init; }
    public required List<TenantFile> TenantFiles { get; init; }
}
