using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;

namespace PoundPupLegacy.EditModel.UI.Services;

public sealed record FileReturn
{
    public required Stream Stream { get; init; }
    public required string FileName { get; init; }
    public required string MimeType { get; init; }
}

public interface IAttachmentStoreService
{
    Task<string?> StoreFile(IBrowserFile file);
    Task<int?> StoreFile(IFormFile file);
}
