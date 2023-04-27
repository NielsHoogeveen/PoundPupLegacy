using OneOf;
using OneOf.Types;

namespace PoundPupLegacy.ViewModel.UI.Services;

public record FileReturn
{
    public required Stream Stream { get; init; }
    public required string FileName { get; init; }
    public required string MimeType { get; init; }
}

public interface IFetchAttachmentService
{
    [RequireNamedArgs]
    Task<OneOf<FileReturn, None, Error<string>>> GetFileStream(int id, int userId, int tenantId);

}
