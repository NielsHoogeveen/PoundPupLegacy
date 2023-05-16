using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class AttachmentStoreService : IAttachmentStoreService
{
    private readonly NpgsqlConnection _connection;
    private readonly ILogger<AttachmentStoreService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEntityCreator<CreateModel.File> _fileCreator;

    public AttachmentStoreService(
        IDbConnection connection,
        IConfiguration configuration,
        ILogger<AttachmentStoreService> logger,
        IEntityCreator<CreateModel.File> fileCreator
        )
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        _connection = (NpgsqlConnection)connection;
        _logger = logger;
        _configuration = configuration;
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
                var fm = new CreateModel.File {
                    Id = null,
                    MimeType = file.ContentType,
                    Name = file.FileName,
                    Path = fileName,
                    Size = (int)file.Length,
                    TenantFiles = new List<TenantFile>()
                };
                await _fileCreator.CreateAsync(new List<CreateModel.File> { fm }.ToAsyncEnumerable(), _connection);
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

}
