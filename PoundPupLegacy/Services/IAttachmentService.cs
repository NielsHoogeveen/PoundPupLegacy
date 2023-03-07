using Microsoft.AspNetCore.Components.Forms;
using OneOf;
using OneOf.Types;

namespace PoundPupLegacy.Services;

public record FileReturn
{
    public required Stream Stream { get; init; }
    public required string FileName { get; init; }
    public required string MimeType { get; init; }
}

public interface IAttachmentService
{
    Task<OneOf<FileReturn,None, Error<string>>> GetFileStream(int id);

    Task<string?> StoreFile(IBrowserFile file);
}
