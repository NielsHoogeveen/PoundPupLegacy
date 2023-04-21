using Microsoft.AspNetCore.Components.Forms;
using OneOf;
using OneOf.Types;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Services;

public record FileReturn
{
    public required Stream Stream { get; init; }
    public required string FileName { get; init; }
    public required string MimeType { get; init; }
}

public interface IAttachmentService
{
    [RequireNamedArgs]
    Task<OneOf<FileReturn, None, Error<string>>> GetFileStream(int id, int userId, int tenantId);

    Task<string?> StoreFile(IBrowserFile file);
    Task<int?> StoreFile(IFormFile file);
}
