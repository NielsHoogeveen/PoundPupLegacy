using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.CreateModel.Updaters;
public interface IEntityUpdater<T>
    where T : NodeToUpdate
{
    Task UpdateAsync(T request, IDbConnection connection);
}

public class NodeDetailsChangerFactory(
    IDatabaseUpdaterFactory<TenantNode.TenantNodeToUpdate> tenantNodeUpdaterFactory,
    IDatabaseDeleterFactory<TenantNodeToDelete> tenantNodeDeleterFactory,
    IDatabaseInserterFactory<TenantNode.TenantNodeToCreateForExistingNode> tenantNodeInserterFactory,
    IDatabaseInserterFactory<NodeTermToAdd> nodeTermInserterFactory,
    IDatabaseDeleterFactory<NodeTermToRemove> nodeTermDeleterFactory
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
    IDatabaseUpdater<TenantNode.TenantNodeToUpdate> tenantNodeUpdater,
    IDatabaseDeleter<TenantNodeToDelete> tenantNodeDeleter,
    IDatabaseInserter<TenantNode.TenantNodeToCreateForExistingNode> tenantNodeInserter,
    IDatabaseInserter<NodeTermToAdd> nodeTermInserter,
    IDatabaseDeleter<NodeTermToRemove> nodeTermDeleter
) : IAsyncDisposable
{

    public async Task Process(NodeToUpdate node)
    {
        foreach (var newNodeTerms in node.NodeDetailsForUpdate.NodeTermsToAdd) {
            await nodeTermInserter.InsertAsync(newNodeTerms);
        }
        foreach (var nodeTermsToRemove in node.NodeDetailsForUpdate.NodeTermsToRemove) {
            await nodeTermDeleter.DeleteAsync(nodeTermsToRemove);
        }
        foreach (var tenantNodeToUpdate in node.NodeDetailsForUpdate.TenantNodesToUpdate) {
            await tenantNodeUpdater.UpdateAsync(tenantNodeToUpdate);
        }
        foreach (var newTenentNode in node.NodeDetailsForUpdate.TenantNodesToAdd) {
            await tenantNodeInserter.InsertAsync(newTenentNode);
        }
        foreach (var tenantNodeToRemove in node.NodeDetailsForUpdate.TenantNodesToRemove) {
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
    where T : NodeToUpdate
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
    where T : NodeToUpdate
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
