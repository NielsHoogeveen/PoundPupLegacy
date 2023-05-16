namespace PoundPupLegacy.CreateModel;

public sealed record TenantFile : IRequest
{
    public required int TenantId { get; init; }
    public required int? FileId { get; set; }
    public required int? TenantFileId { get; set; }
}
