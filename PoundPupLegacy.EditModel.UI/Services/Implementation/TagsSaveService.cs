namespace PoundPupLegacy.EditModel.UI.Services.Implementation;

internal sealed class TagsSaveService(
        IDatabaseDeleterFactory<NodeTermDeleterRequest> nodeTermDeleterFactory,
        IDatabaseInserterFactory<CreateModel.NodeTerm> nodeTermInserterFactory
        ) : ISaveService<IEnumerable<Tag>>
{
    public async Task SaveAsync(IEnumerable<Tag> tags, IDbConnection connection)
    {
        await using var deleter = await nodeTermDeleterFactory.CreateAsync(connection);
        foreach (var tag in tags.Where(x => x.HasBeenDeleted)) {
            await deleter.DeleteAsync(new NodeTermDeleterRequest {
                NodeId = tag.NodeId!.Value,
                TermId = tag.TermId
            });
        }

        await using var inserter = await nodeTermInserterFactory.CreateAsync(connection);
        foreach (var tag in tags.Where(x => !x.IsStored)) {
            await inserter.InsertAsync(new CreateModel.NodeTerm {
                NodeId = tag.NodeId!.Value,
                TermId = tag.TermId
            });
        }
    }
}
