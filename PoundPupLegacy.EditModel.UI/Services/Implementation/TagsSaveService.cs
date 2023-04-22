using PoundPupLegacy.EditModel.Deleters;
using System.Data;

namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class TagsSaveService : ISaveService<IEnumerable<Tag>>
{
    private readonly IDatabaseDeleterFactory<NodeTermDeleterRequest> _nodeTermDeleterFactory;
    private readonly IDatabaseInserterFactory<CreateModel.NodeTerm> _nodeTermInserterFactory;
    public TagsSaveService(
        IDatabaseDeleterFactory<NodeTermDeleterRequest> nodeTermDeleterFactory,
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
            await deleter.DeleteAsync(new NodeTermDeleterRequest {
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
