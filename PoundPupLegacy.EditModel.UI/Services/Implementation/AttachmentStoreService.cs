using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class AttachmentStoreService(
    NpgsqlDataSource dataSource,
    ILogger<AttachmentStoreService> logger,
    IConfiguration configuration,
    IEntityCreatorFactory<DomainModel.File> fileCreatorFactory
) : DatabaseService(dataSource, logger), IAttachmentStoreService
{
    public async Task<int?> StoreFile(IFormFile file)
    {
        var attachmentsLocation = configuration["AttachmentsLocation"];
        if (attachmentsLocation is null) {
            logger.LogError("AttachmentsLocation is not defined in appsettings.json");
            return null;
        }
        var maxFileSizeString = configuration["MaxFileSize"];
        if (maxFileSizeString is null) {
            logger.LogError("Max file size is not defined in appsettings.json");
            return null;
        }
        if (int.TryParse(maxFileSizeString, out int maxFileSize)) {
            return await WithTransactedConnection(async (connection) => {
                var fileName = Guid.NewGuid().ToString();
                var fullName = Path.Combine(attachmentsLocation, fileName);
                logger.Log(LogLevel.Information, "starting to write file {0}", fullName);
                await using FileStream fs = new(fullName, FileMode.Create);
                await file.CopyToAsync(fs);
                var fm = new DomainModel.File {
                    Identification = new Identification.Possible {
                        Id = null,
                    },
                    MimeType = file.ContentType,
                    Name = file.FileName,
                    Path = fileName,
                    Size = (int)file.Length,
                    TenantFiles = new List<TenantFile>()
                };
                await using var fileCreator = await fileCreatorFactory.CreateAsync(connection);
                await fileCreator.CreateAsync(new List<DomainModel.File> { fm }.ToAsyncEnumerable());
                logger.Log(LogLevel.Information, "created file with id {0}", fm.Name);
                return fm.Identification.Id;
            });
        }
        else {
            logger.LogError("Max file size as defined in appsettings.json is not a number");
            return null;
        }
    }
    public async Task<string?> StoreFile(IBrowserFile file)
    {
        var attachmentsLocation = configuration["AttachmentsLocation"];
        if (attachmentsLocation is null) {
            logger.LogError("AttachmentsLocation is not defined in appsettings.json");
            return null;
        }
        var maxFileSizeString = configuration["MaxFileSize"];
        if (maxFileSizeString is null) {
            logger.LogError("Max file size is not defined in appsettings.json");
            return null;
        }
        if (int.TryParse(maxFileSizeString, out int maxFileSize)) {
            try {
                var fileName = Guid.NewGuid().ToString();
                var fullName = Path.Combine(attachmentsLocation, fileName);
                logger.Log(LogLevel.Information, "starting to write file {0}", fullName);
                await using FileStream fs = new(fullName, FileMode.Create);
                await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
                return fileName;
            }catch(Exception ex) {
                logger.LogError(ex, "Error while storing file");
                return null;
            }
        }
        else {
            logger.LogError("Max file size as defined in appsettings.json is not a number");
            return null;
        }
    }

}
