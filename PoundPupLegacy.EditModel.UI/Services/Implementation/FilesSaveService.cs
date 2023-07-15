using PoundPupLegacy.DomainModel.Deleters;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class FilesSaveService(
    IDatabaseDeleterFactory<FileDeleterRequest> fileDeleterFactory,
    IDatabaseInserterFactory<FileInserterRequest> fileInserterFactory
) : ISaveService<IEnumerable<File>>
{
    public async Task SaveAsync(IEnumerable<File> attachments, IDbConnection connection)
    {
        if (attachments.Any(x => x.HasBeenDeleted)) {
            await using var deleter = await fileDeleterFactory.CreateAsync(connection);
            foreach (var attachment in attachments.Where(x => x.HasBeenDeleted)) {
                await deleter.DeleteAsync(new FileDeleterRequest {
                    FileId = attachment.Id!.Value,
                    NodeId = attachment.NodeId!.Value
                });
            }
        }
        if (attachments.Any(x => x.Id is null)) {
            await using var inserter = await fileInserterFactory.CreateAsync(connection);
            foreach (var attachment in attachments.Where(x => x.Id is null)) {
                await inserter.InsertAsync(new FileInserterRequest {
                    MimeType = attachment.MimeType,
                    Path = attachment.Path,
                    Size = attachment.Size,
                    Name = attachment.Name,
                    NodeId = attachment.NodeId!.Value
                });
            }
        }
    }
}
