using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.CreateModel.Updaters;
public interface IEntityUpdater<T>
    where T : ImmediatelyIdentifiableNode
{
    Task UpdateAsync(T request, IDbConnection connection);
}

public abstract class EntityUpdater<T>(
    IDatabaseUpdaterFactory<ImmediatelyIdentifiableTenantNode> tenantNodeUpdater,
    IDatabaseDeleterFactory<ImmediatelyIdentifiableTenantNode> tenantNodeDeleter,
    IDatabaseInserterFactory<NewTenantNodeForExistingNode> tenantNodeInserter,
    IDatabaseInserterFactory<NodeTerm> nodeTermInserter,
    IDatabaseDeleterFactory<NodeTerm> nodeTermDeleter
)
    where T : ImmediatelyIdentifiableNode
{

    protected abstract Task UpdateEntityAsync(T request, IDbConnection connection);

    public async Task UpdateAsync(T request, IDbConnection connection)
    {
        await UpdateEntityAsync(request, connection);
        await UpdateTenantNodesAsync(request, connection);
        await UpdateNodeTermsAsync(request, connection);
    }
    private async Task UpdateNodeTermsAsync(T request, IDbConnection connection)
    {
        await using var inserter = await nodeTermInserter.CreateAsync(connection);
        await using var deleter = await nodeTermDeleter.CreateAsync(connection);
        foreach (var newNodeTerms in request.NewNodeTerms) {
            await inserter.InsertAsync(newNodeTerms);
        }
        foreach (var nodeTermsToRemove in request.NodeTermsToRemove) {
            await deleter.DeleteAsync(nodeTermsToRemove);
        }
    }
    private async Task UpdateTenantNodesAsync(T request, IDbConnection connection)
    {
        await using var updater = await tenantNodeUpdater.CreateAsync(connection);
        await using var inserter = await tenantNodeInserter.CreateAsync(connection);
        await using var deleter = await tenantNodeDeleter.CreateAsync(connection);
        foreach (var tenantNodeToUpdate in request.TenantNodesToUpdate) {
            await updater.UpdateAsync(tenantNodeToUpdate);
        }
        foreach (var newTenentNode in request.NewTenantNodes) {
            await inserter.InsertAsync(newTenentNode);
        }
        foreach (var tenantNodeToRemove in request.TenantNodesToRemove) {
            await deleter.DeleteAsync(tenantNodeToRemove);
        }
    }
}
