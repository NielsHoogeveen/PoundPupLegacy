namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class DiscussionCreator : EntityCreator<Discussion>
{
    private readonly IDatabaseInserterFactory<Discussion> _discussionInserterFactory;
    private readonly IDatabaseInserterFactory<Node> _nodeInserterFactory;
    private readonly IDatabaseInserterFactory<Searchable> _searchableInserterFactory;
    private readonly IDatabaseInserterFactory<SimpleTextNode> _simpleTextNodeInserterFactory;
    private readonly IDatabaseInserterFactory<TenantNode> _tenantNodeInserterFactory;
    public DiscussionCreator(IDatabaseInserterFactory<Discussion> discussionInserterFactory, IDatabaseInserterFactory<Node> nodeInserterFactory, IDatabaseInserterFactory<Searchable> searchableInserterFactory, IDatabaseInserterFactory<SimpleTextNode> simpleTextNodeInserterFactory, IDatabaseInserterFactory<TenantNode> tenantNodeInserterFactory)
    {
        _discussionInserterFactory = discussionInserterFactory;
        _nodeInserterFactory = nodeInserterFactory;
        _searchableInserterFactory = searchableInserterFactory;
        _simpleTextNodeInserterFactory = simpleTextNodeInserterFactory;
        _tenantNodeInserterFactory = tenantNodeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<Discussion> discussions, IDbConnection connection)
    {

        await using var nodeWriter = await _nodeInserterFactory.CreateAsync(connection);
        await using var searchableWriter = await _searchableInserterFactory.CreateAsync(connection);
        await using var simpleTextNodeWriter = await _simpleTextNodeInserterFactory.CreateAsync(connection);
        await using var discussionWriter = await _discussionInserterFactory.CreateAsync(connection);
        await using var tenantNodeWriter = await _tenantNodeInserterFactory.CreateAsync(connection);


        await foreach (var discussion in discussions) {
            await nodeWriter.InsertAsync(discussion);
            await searchableWriter.InsertAsync(discussion);
            await simpleTextNodeWriter.InsertAsync(discussion);
            await discussionWriter.InsertAsync(discussion);
            foreach (var tenantNode in discussion.TenantNodes) {
                tenantNode.NodeId = discussion.Id;
                await tenantNodeWriter.InsertAsync(tenantNode);
            }

        }
    }
}
