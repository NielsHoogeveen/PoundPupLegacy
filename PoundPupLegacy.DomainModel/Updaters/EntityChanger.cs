using PoundPupLegacy.DomainModel.Creators;
using PoundPupLegacy.DomainModel.Deleters;

namespace PoundPupLegacy.DomainModel.Updaters;
public interface IEntityUpdater<T>
    where T : NodeToUpdate
{
    Task UpdateAsync(T request, IDbConnection connection);
}

public class NodeDetailsChangerFactory(
    IDatabaseUpdaterFactory<TenantNode.ToUpdate> tenantNodeUpdaterFactory,
    IDatabaseDeleterFactory<TenantNodeToDelete> tenantNodeDeleterFactory,
    IDatabaseInserterFactory<TenantNode.ToCreate.ForExistingNode> tenantNodeInserterFactory,
    IDatabaseInserterFactory<ResolvedNodeTermToAdd> nodeTermInserterFactory,
    IDatabaseDeleterFactory<NodeTermToRemove> nodeTermDeleterFactory,
    IEntityCreatorFactory<File> fileCreatorFactory,
    IEntityCreatorFactory<NodeFile> nodeFileCreatorFactory,
    IDatabaseDeleterFactory<FileDeleterRequest> fileDeleterFactory
)
{
    public async Task<NodeDetailsChanger> CreateAsync(IDbConnection connection)
    {
        return new NodeDetailsChanger(
            await tenantNodeUpdaterFactory.CreateAsync(connection),
            await tenantNodeDeleterFactory.CreateAsync(connection),
            await tenantNodeInserterFactory.CreateAsync(connection),
            await nodeTermInserterFactory.CreateAsync(connection),
            await nodeTermDeleterFactory.CreateAsync(connection),
            await fileCreatorFactory.CreateAsync(connection),
            await nodeFileCreatorFactory.CreateAsync(connection),
            await fileDeleterFactory.CreateAsync(connection)
        );
    }
}

public class NodeDetailsChanger(
    IDatabaseUpdater<TenantNode.ToUpdate> tenantNodeUpdater,
    IDatabaseDeleter<TenantNodeToDelete> tenantNodeDeleter,
    IDatabaseInserter<TenantNode.ToCreate.ForExistingNode> tenantNodeInserter,
    IDatabaseInserter<ResolvedNodeTermToAdd> nodeTermInserter,
    IDatabaseDeleter<NodeTermToRemove> nodeTermDeleter,
    IEntityCreator<File> fileCreator,
    IEntityCreator<NodeFile> nodeFileCreator,
    IDatabaseDeleter<FileDeleterRequest> fileDeleter
) : IAsyncDisposable
{

    public async Task Process(NodeToUpdate node)
    {
        foreach (var newNodeTerms in node.NodeDetails.NodeTermsToAdd) {
            await nodeTermInserter.InsertAsync(newNodeTerms);
        }
        foreach (var nodeTermsToRemove in node.NodeDetails.NodeTermsToRemove) {
            await nodeTermDeleter.DeleteAsync(nodeTermsToRemove);
        }
        foreach (var tenantNodeToUpdate in node.NodeDetails.TenantNodesToUpdate) {
            await tenantNodeUpdater.UpdateAsync(tenantNodeToUpdate);
        }
        foreach (var newTenentNode in node.NodeDetails.TenantNodesToAdd) {
            await tenantNodeInserter.InsertAsync(newTenentNode);
        }
        foreach (var tenantNodeToRemove in node.NodeDetails.TenantNodesToRemove) {
            await tenantNodeDeleter.DeleteAsync(tenantNodeToRemove);
        }
        foreach (var file in node.NodeDetails.FilesToAdd) {
            await fileCreator.CreateAsync(file);
            var fileId = file.Identification.Id;
            await nodeFileCreator.CreateAsync(new NodeFile {
                NodeId = node.Identification.Id,
                FileId = fileId!.Value
            });
        }
        foreach(var file in node.NodeDetails.FileIdsToRemove) {
            await fileDeleter.DeleteAsync(new FileDeleterRequest { FileId = file, NodeId = node.Identification.Id });
        }
    }
    public async ValueTask DisposeAsync()
    {
        await tenantNodeUpdater.DisposeAsync();
        await tenantNodeInserter.DisposeAsync();
        await tenantNodeDeleter.DisposeAsync();
        await nodeTermInserter.DisposeAsync();
        await nodeTermDeleter.DisposeAsync();
        await fileCreator.DisposeAsync();
        await nodeFileCreator.DisposeAsync();
        await fileDeleter.DisposeAsync();
    }
}


public interface IEntityChangerFactory<T>
{
    Task<IEntityChanger<T>> CreateAsync(IDbConnection connection);
}

public interface IEntityChanger<T> : IAsyncDisposable
{
    Task UpdateAsync(T request);
}

public class CaseChanger<T>(
    IDatabaseUpdater<T> databaseUpdater,
    CaseDetailsChanger caseDetailsChanger,
    NodeDetailsChanger nodeDetailsChanger,
    IDatabaseUpdater<LocationUpdaterRequest> locationUpdater,
    LocatableDetailsCreator locatableDetailsCreator
) : LocatableChanger<T>(databaseUpdater, nodeDetailsChanger, locationUpdater, locatableDetailsCreator)
where T : CaseToUpdate
{
    protected override async Task Process(T request)
    {
        await base.Process(request);
        await caseDetailsChanger.Process(request);
    }
}
public class LocatableChanger<T>(
    IDatabaseUpdater<T> databaseUpdater,
    NodeDetailsChanger nodeDetailsChanger,
    IDatabaseUpdater<LocationUpdaterRequest> locationUpdater,
    LocatableDetailsCreator locatableDetailsCreator
) : NodeChanger<T>(databaseUpdater, nodeDetailsChanger)
where T : LocatableToUpdate
{
    protected override async Task Process(T request)
    {
        await base.Process(request);
        foreach (var location in request.LocatableDetails.LocationsToUpdate) {
            await locationUpdater.UpdateAsync(new LocationUpdaterRequest {
                Additional = location.Additional,
                City = location.City,
                CountryId = location.CountryId,
                Id = location.Identification.Id,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                PostalCode = location.PostalCode,
                Street = location.Street,
                SubdivisionId = location.SubdivisionId,
            });
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await locationUpdater.DisposeAsync();
        await locatableDetailsCreator.DisposeAsync();
    }
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
) : IAsyncDisposable, IEntityChanger<T>
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
