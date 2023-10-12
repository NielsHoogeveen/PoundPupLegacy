using PoundPupLegacy.DomainModel.Creators;
using PoundPupLegacy.DomainModel.Deleters;

namespace PoundPupLegacy.DomainModel.Updaters;
public interface IEntityUpdater<T>
    where T : NodeToUpdate
{
    Task UpdateAsync(T request, IDbConnection connection);
}

public class NodeDetailsChangerFactory(
    IDatabaseUpdaterFactory<NodeDetails.ForUpdate> nodeDetailsUpdaterFactory,
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
            await nodeDetailsUpdaterFactory.CreateAsync(connection),
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
    IDatabaseUpdater<NodeDetails.ForUpdate> nodeDetailsUpdater,
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
        await nodeDetailsUpdater.UpdateAsync(node.NodeDetails);
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
        await nodeDetailsUpdater.DisposeAsync();
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
    IDatabaseInserter<Location.ToCreate> locationInserter,
    IDatabaseInserter<LocationLocatable> locationLocatableInserter,
    IDatabaseUpdater<Term.ToUpdate> termUpdater
) : LocatableChanger<T>(databaseUpdater, nodeDetailsChanger, locationUpdater, locationInserter, locationLocatableInserter, termUpdater)
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
    IDatabaseInserter<Location.ToCreate> locationInserter,
    IDatabaseInserter<LocationLocatable> locationLocatableInserter,
    IDatabaseUpdater<Term.ToUpdate> termUpdater
) : NameableChanger<T>(databaseUpdater, nodeDetailsChanger, termUpdater)
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
        foreach (var location in request.LocatableDetails.LocationsToAdd) {
            await locationInserter.InsertAsync(location);
            await locationLocatableInserter.InsertAsync(new LocationLocatable {
                LocatableId = request.Identification.Id,
                LocationId = location.Identification.Id!.Value,
            });
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await locationUpdater.DisposeAsync();
        await locationInserter.DisposeAsync();
        await locationLocatableInserter.DisposeAsync();
    }
}
public class NameableChanger<T>(
    IDatabaseUpdater<T> databaseUpdater,
    NodeDetailsChanger nodeDetailsChanger,
    IDatabaseUpdater<Term.ToUpdate> termUpdater
) : NodeChanger<T>(databaseUpdater, nodeDetailsChanger)
where T : LocatableToUpdate
{
    protected override async Task Process(T request)
    {
        await base.Process(request);
        await termUpdater.UpdateAsync(request.NameableDetails.TermToUpdate);
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await termUpdater.DisposeAsync();
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
        request.NodeDetails.ChangedDateTime = DateTime.Now;
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
