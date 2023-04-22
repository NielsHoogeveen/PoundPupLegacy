using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using OneOf;
using OneOf.Types;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;

namespace PoundPupLegacy.ViewModel.UI.Services.Implementation;

internal sealed class FetchAttachmentService : IFetchAttachmentService
{
    private readonly NpgsqlConnection _connection;
    private readonly ILogger<FetchAttachmentService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ISingleItemDatabaseReaderFactory<FileDocumentReaderRequest, Models.File> _fileDocumentReaderFactory;

    public FetchAttachmentService(
        IDbConnection connection,
        IConfiguration configuration,
        ILogger<FetchAttachmentService> logger,
        ISingleItemDatabaseReaderFactory<FileDocumentReaderRequest, Models.File> fileDocumentReaderFactory
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;
        _logger = logger;
        _configuration = configuration;
        _fileDocumentReaderFactory = fileDocumentReaderFactory;
    }


    public async Task<OneOf<FileReturn, None, Error<string>>> GetFileStream(int fileId, int userId, int tenantId)
    {
        try {
            await _connection.OpenAsync();
            var attachementsLocation = _configuration["AttachmentsLocation"];
            if (attachementsLocation is null) {
                _logger.LogError("AttachmentsLocation is not defined in appsettings.json");
                return new None();
            }
            await using var reader = await _fileDocumentReaderFactory.CreateAsync(_connection);
            var file = await reader.ReadAsync(new FileDocumentReaderRequest {
                FileId = fileId,
                UserId = userId,
                TenantId = tenantId
            });
            if (file is null)
                return new None();
            var fullPath = attachementsLocation + "\\" + file.Path;
            return new FileReturn { FileName = file.Name, MimeType = file.MimeType, Stream = System.IO.File.OpenRead(fullPath) };
        }
        catch (Exception e) {
            return new Error<string>(e.Message);
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
