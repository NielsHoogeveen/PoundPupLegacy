namespace PoundPupLegacy.CreateModel;

public record TenantFile
{
    public required int TenantId { get; init; }
    public required int? FileId { get; set; }
    public required int? TenantFileId { get; set; }
}
