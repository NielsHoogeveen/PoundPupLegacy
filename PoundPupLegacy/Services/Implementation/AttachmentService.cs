using Microsoft.AspNetCore.Components.Forms;
using Npgsql;
using OneOf;
using OneOf.Types;
using PoundPupLegacy.Common;
using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.ViewModel.Readers;
using System.Data;
using File = PoundPupLegacy.CreateModel.File;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class AttachmentService : IAttachmentService
{
    private readonly NpgsqlConnection _connection;
    private readonly ILogger<AttachmentService> _logger;
    private readonly IConfiguration _configuration;
    private readonly ISingleItemDatabaseReaderFactory<FileDocumentReaderRequest, ViewModel.File> _fileDocumentReaderFactory;
    private readonly IEntityCreator<File> _fileCreator;

    public AttachmentService(
        IDbConnection connection,
        IConfiguration configuration,
        ILogger<AttachmentService> logger,
        ISingleItemDatabaseReaderFactory<FileDocumentReaderRequest, ViewModel.File> fileDocumentReaderFactory,
        IEntityCreator<File> fileCreator
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;
        _logger = logger;
        _configuration = configuration;
        _fileDocumentReaderFactory = fileDocumentReaderFactory;
        _fileCreator = fileCreator;
    }

    public async Task<int?> StoreFile(IFormFile file)
    {
        var attachmentsLocation = _configuration["AttachmentsLocation"];
        if (attachmentsLocation is null) {
            _logger.LogError("AttachmentsLocation is not defined in appsettings.json");
            return null;
        }
        var maxFileSizeString = _configuration["MaxFileSize"];
        if (maxFileSizeString is null) {
            _logger.LogError("Max file size is not defined in appsettings.json");
            return null;
        }
        if (int.TryParse(maxFileSizeString, out int maxFileSize)) {
            await _connection.OpenAsync();
            try {
                var fileName = Guid.NewGuid().ToString();
                var fullName = attachmentsLocation + "\\" + fileName;
                await using FileStream fs = new(fullName, FileMode.Create);
                await file.CopyToAsync(fs);
                var fm = new File {
                    Id = null,
                    MimeType = file.ContentType,
                    Name = file.FileName,
                    Path = fileName,
                    Size = (int)file.Length,
                    TenantFiles = new List<TenantFile>()
                };
                await _fileCreator.CreateAsync(new List<File> { fm }.ToAsyncEnumerable(), _connection);
                return fm.Id;
            }
            finally {
                await _connection.CloseAsync();
            }
        }
        else {
            _logger.LogError("Max file size as defined in appsettings.json is not a number");
            return null;
        }
    }
    public async Task<string?> StoreFile(IBrowserFile file)
    {
        var attachmentsLocation = _configuration["AttachmentsLocation"];
        if (attachmentsLocation is null) {
            _logger.LogError("AttachmentsLocation is not defined in appsettings.json");
            return null;
        }
        var maxFileSizeString = _configuration["MaxFileSize"];
        if (maxFileSizeString is null) {
            _logger.LogError("Max file size is not defined in appsettings.json");
            return null;
        }
        if (int.TryParse(maxFileSizeString, out int maxFileSize)) {
            var fileName = Guid.NewGuid().ToString();
            var fullName = attachmentsLocation + "\\" + fileName;
            await using FileStream fs = new(fullName, FileMode.Create);
            await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
            return fileName;
        }
        else {
            _logger.LogError("Max file size as defined in appsettings.json is not a number");
            return null;
        }
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
