using PoundPupLegacy.Common;
using PoundPupLegacy.Deleters;
using PoundPupLegacy.Inserters;
using System.Data;
using File = PoundPupLegacy.EditModel.File;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class FilesSaveService : ISaveService<IEnumerable<File>>
{
    private readonly IDatabaseDeleterFactory<FileDeleterRequest> _fileDeleterFactory;
    private readonly IDatabaseInserterFactory<FileInserterRequest> _fileInserterFactory;

    public FilesSaveService(
        IDatabaseDeleterFactory<FileDeleterRequest> fileDeleterFactory,
        IDatabaseInserterFactory<FileInserterRequest> fileInserterFactory
        )
    {
        _fileDeleterFactory = fileDeleterFactory;
        _fileInserterFactory = fileInserterFactory;
    }
    public async Task SaveAsync(IEnumerable<File> attachments, IDbConnection connection)
    {
        if (attachments.Any(x => x.HasBeenDeleted)) {
            await using var deleter = await _fileDeleterFactory.CreateAsync(connection);
            foreach (var attachment in attachments.Where(x => x.HasBeenDeleted)) {
                await deleter.DeleteAsync(new FileDeleterRequest {
                    FileId = attachment.Id!.Value,
                    NodeId = attachment.NodeId!.Value
                });
            }
        }
        if (attachments.Any(x => x.Id is null)) {
            await using var inserter = await _fileInserterFactory.CreateAsync(connection);
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
