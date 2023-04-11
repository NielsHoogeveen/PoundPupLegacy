using PoundPupLegacy.Common;
using PoundPupLegacy.Deleters;
using PoundPupLegacy.EditModel;
using System.Data;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class TagsSaveService : ISaveService<IEnumerable<Tag>>
{
    private readonly IDatabaseDeleterFactory<NodeTermDeleter> _nodeTermDeleterFactory;
    private readonly IDatabaseInserterFactory<CreateModel.NodeTerm> _nodeTermInserterFactory;
    public TagsSaveService(
        IDatabaseDeleterFactory<NodeTermDeleter> nodeTermDeleterFactory,
        IDatabaseInserterFactory<CreateModel.NodeTerm> nodeTermInserterFactory
        )
    {
        _nodeTermDeleterFactory = nodeTermDeleterFactory;
        _nodeTermInserterFactory = nodeTermInserterFactory;
    }
    public async Task SaveAsync(IEnumerable<Tag> tags, IDbConnection connection)
    {
        await using var deleter = await _nodeTermDeleterFactory.CreateAsync(connection);
        foreach (var tag in tags.Where(x => x.HasBeenDeleted)) {
            await deleter.DeleteAsync(new NodeTermDeleter.Request {
                NodeId = tag.NodeId!.Value,
                TermId = tag.TermId
            });
        }

        await using var inserter = await _nodeTermInserterFactory.CreateAsync(connection);
        foreach (var tag in tags.Where(x => !x.IsStored)) {
            await inserter.InsertAsync(new CreateModel.NodeTerm {
                NodeId = tag.NodeId!.Value,
                TermId = tag.TermId
            });
        }
    }
}
