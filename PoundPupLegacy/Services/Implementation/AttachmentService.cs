using Microsoft.AspNetCore.Components.Forms;
using Npgsql;
using OneOf;
using OneOf.Types;
using PoundPupLegacy.Common;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;
using PoundPupLegacy.ViewModel.Readers;

namespace PoundPupLegacy.Services.Implementation;

public class AttachmentService : IAttachmentService
{
    private readonly NpgsqlConnection _connection;
    private readonly ILogger<AttachmentService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IDatabaseReaderFactory<FileDocumentReader> _fileDocumentReaderFactory;

    public AttachmentService(
        NpgsqlConnection connection,
        IConfiguration configuration,
        ILogger<AttachmentService> logger,
        IDatabaseReaderFactory<FileDocumentReader> fileDocumentReaderFactory)
    {
        _connection = connection;
        _logger = logger;
        _configuration = configuration;
        _fileDocumentReaderFactory = fileDocumentReaderFactory;
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
                var fm = new Model.File {
                    Id = null,
                    MimeType = file.ContentType,
                    Name = file.FileName,
                    Path = fileName,
                    Size = (int)file.Length,
                    TenantFiles = new List<TenantFile>()
                };
                await FileCreator.CreateAsync(new List<Model.File> { fm }.ToAsyncEnumerable(), _connection);
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
            var file = await reader.ReadAsync(new FileDocumentReader.FileDocumentRequest {
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
            if (_connection.State == System.Data.ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
