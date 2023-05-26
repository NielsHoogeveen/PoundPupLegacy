namespace PoundPupLegacy.CreateModel.Updaters;
public interface IEntityUpdater<T>
    where T : ImmediatelyIdentifiableNode
{
    Task UpdateAsync(T request, IDbConnection connection);
}

public class NodeDetailsChangerFactory(
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableTenantNode> tenantNodeUpdaterFactory,
    IDatabaseDeleterFactory<ImmediatelyIdentifiableTenantNode> tenantNodeDeleterFactory,
    IDatabaseInserterFactory<NewTenantNodeForExistingNode> tenantNodeInserterFactory,
    IDatabaseInserterFactory<NodeTerm> nodeTermInserterFactory,
    IDatabaseDeleterFactory<NodeTerm> nodeTermDeleterFactory
)
{
    public async Task<NodeDetailsChanger> CreateAsync(IDbConnection connection)
    {
        return new NodeDetailsChanger(
            await tenantNodeUpdaterFactory.CreateAsync(connection),
            await tenantNodeDeleterFactory.CreateAsync(connection),
            await tenantNodeInserterFactory.CreateAsync(connection),
            await nodeTermInserterFactory.CreateAsync(connection),
            await nodeTermDeleterFactory.CreateAsync(connection)
        );
    } 
}

public class NodeDetailsChanger(
    IDatabaseUpdater<ImmediatelyIdentifiableTenantNode> tenantNodeUpdater,
    IDatabaseDeleter<ImmediatelyIdentifiableTenantNode> tenantNodeDeleter,
    IDatabaseInserter<NewTenantNodeForExistingNode> tenantNodeInserter,
    IDatabaseInserter<NodeTerm> nodeTermInserter,
    IDatabaseDeleter<NodeTerm> nodeTermDeleter
) : IAsyncDisposable
{

    public async Task Process(ImmediatelyIdentifiableNode node)
    {
        foreach (var newNodeTerms in node.NewNodeTerms) {
            await nodeTermInserter.InsertAsync(newNodeTerms);
        }
        foreach (var nodeTermsToRemove in node.NodeTermsToRemove) {
            await nodeTermDeleter.DeleteAsync(nodeTermsToRemove);
        }
        foreach (var tenantNodeToUpdate in node.TenantNodesToUpdate) {
            await tenantNodeUpdater.UpdateAsync(tenantNodeToUpdate);
        }
        foreach (var newTenentNode in node.NewTenantNodes) {
            await tenantNodeInserter.InsertAsync(newTenentNode);
        }
        foreach (var tenantNodeToRemove in node.TenantNodesToRemove) {
            await tenantNodeDeleter.DeleteAsync(tenantNodeToRemove);
        }
    }

    public async ValueTask DisposeAsync()
    {
        await tenantNodeUpdater.DisposeAsync();
        await tenantNodeInserter.DisposeAsync();
        await tenantNodeDeleter.DisposeAsync();
        await nodeTermInserter.DisposeAsync();
        await nodeTermDeleter.DisposeAsync();
    }
}


public interface IEntityChangerFactory<T>
{
    Task<IEntityChanger<T>> CreateAsync(IDbConnection connection);
}

public interface IEntityChanger<T>
{
    Task UpdateAsync(T request);
}

public class NodeChanger<T>(
    IDatabaseUpdater<T> databaseUpdater,
    NodeDetailsChanger nodeDetailsChanger
) : EntityChanger<T>()
    where T : ImmediatelyIdentifiableNode
{
    protected override async Task Process(T request)
    {
        await base.Process(request);
        await databaseUpdater.UpdateAsync(request);
        await nodeDetailsChanger.Process(request);
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await databaseUpdater.DisposeAsync();
        await nodeDetailsChanger.DisposeAsync();
    }
}

public class EntityChanger<T>(
): IAsyncDisposable, IEntityChanger<T>
    where T : ImmediatelyIdentifiableNode
{
    protected virtual async Task Process(T request) 
    {
        await Task.CompletedTask;
    }
    public Task UpdateAsync(T request) 
    { 
        return Process(request);
    }

    public virtual async ValueTask DisposeAsync()
    {
        await Task.CompletedTask;
    }
}
