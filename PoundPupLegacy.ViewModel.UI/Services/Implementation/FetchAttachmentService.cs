using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OneOf;
using OneOf.Types;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchAttachmentService(
    IDbConnection connection,
    IConfiguration configuration,
    ILogger<FetchAttachmentService> logger,
    ISingleItemDatabaseReaderFactory<FileDocumentReaderRequest, Models.File> fileDocumentReaderFactory
) : DatabaseService(connection, logger), IFetchAttachmentService
{

    public async Task<OneOf<FileReturn, None, Error<string>>> GetFileStream(int fileId, int userId, int tenantId)
    {
        var attachementsLocation = configuration["AttachmentsLocation"];
        if (attachementsLocation is null) {
            _logger.LogError("AttachmentsLocation is not defined in appsettings.json");
            return new None();
        }

        return await WithConnection<OneOf<FileReturn, None, Error<string>>>(async (connection) => {
            await using var reader = await fileDocumentReaderFactory.CreateAsync(connection);
            var file = await reader.ReadAsync(new FileDocumentReaderRequest {
                FileId = fileId,
                UserId = userId,
                TenantId = tenantId
            });
            if (file is null)
                return new None();
            var fullPath = Path.Combine(attachementsLocation,file.Path);
            return new FileReturn { FileName = file.Name, MimeType = file.MimeType, Stream = System.IO.File.OpenRead(fullPath) };
        });
    }
}
