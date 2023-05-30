using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class AttachmentStoreService(
    IDbConnection connection,
    ILogger<AttachmentStoreService> logger,
    IConfiguration configuration,
    IEntityCreatorFactory<CreateModel.File> fileCreatorFactory
) : DatabaseService(connection, logger), IAttachmentStoreService
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
                var fullName = attachmentsLocation + "\\" + fileName;
                await using FileStream fs = new(fullName, FileMode.Create);
                await file.CopyToAsync(fs);
                var fm = new CreateModel.File {
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
                await fileCreator.CreateAsync(new List<CreateModel.File> { fm }.ToAsyncEnumerable());
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
            var fileName = Guid.NewGuid().ToString();
            var fullName = attachmentsLocation + "\\" + fileName;
            await using FileStream fs = new(fullName, FileMode.Create);
            await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
            return fileName;
        }
        else {
            logger.LogError("Max file size as defined in appsettings.json is not a number");
            return null;
        }
    }

}
