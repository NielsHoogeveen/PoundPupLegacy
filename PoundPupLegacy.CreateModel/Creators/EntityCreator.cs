namespace PoundPupLegacy.CreateModel.Creators;

public interface IEntityCreator<T>: IAsyncDisposable
{
    Task CreateAsync(IAsyncEnumerable<T> elements);
    Task CreateAsync(T element);
}
public interface IEntityCreatorFactory<T>
    where T : class, IRequest
{
    Task<IEntityCreator<T>> CreateAsync(IDbConnection connection);
}
public class LocatableDetailsCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableLocation> locationInserterFactory,
    IDatabaseInserterFactory<LocationLocatable> locationLocatableInserterFactory
)
{
    public async Task<LocatableDetailsCreator> CreateAsync(IDbConnection connection)
    {
        return new LocatableDetailsCreator(
            await locationInserterFactory.CreateAsync(connection),
            await locationLocatableInserterFactory.CreateAsync(connection)
        );
    }
}
public class TermCreatorFactory(
    IDatabaseInserterFactory<TermToCreateForExistingNameable> termInserterFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory
)
{
    public async Task<TermCreator> CreateAsync(IDbConnection connection)
    {
        return new TermCreator(
            new() {
                await termInserterFactory.CreateAsync(connection),
            },
            await termHierarchyInserterFactory.CreateAsync(connection)
        );
    }
}
public class LocatableDetailsCreator(
    IDatabaseInserter<EventuallyIdentifiableLocation> locationInserter,
    IDatabaseInserter<LocationLocatable> locationLocatableInserter
) : IAsyncDisposable
{
    public async Task Process(LocatableToCreate locatable)
    {
        foreach(var location in locatable.LocatableDetails.Locations) {
            await locationInserter.InsertAsync(location);
            await locationLocatableInserter.InsertAsync(new LocationLocatable {
                LocatableId = locatable.Identification.Id!.Value,
                LocationId = location.Identification.Id!.Value,
            });
        }
    }

    public async ValueTask DisposeAsync()
    {
        await locationInserter.DisposeAsync();
        await locationLocatableInserter.DisposeAsync();
    }
}
public class TermCreator(
    List<IDatabaseInserter<TermToCreateForExistingNameable>> inserters,
    IDatabaseInserter<TermHierarchy> termHierarchyInserter
) : InsertingEntityCreator<TermToCreateForExistingNameable>(inserters)
{
    public override async Task ProcessAsync(TermToCreateForExistingNameable element)
    {
        await base.ProcessAsync(element);
        foreach (var parent in element.ParentTermIds) {
            await termHierarchyInserter.InsertAsync(new TermHierarchy 
            { 
                TermIdPartent = parent, 
                TermIdChild = element.Identification.Id!.Value 
            });
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await termHierarchyInserter.DisposeAsync();
    }
}
public class CaseCreator<T>(
    List<IDatabaseInserter<T>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    TermCreator nameableDetailsCreator,
    LocatableDetailsCreator locatableDetailsCreator,
    IEntityCreator<CaseCaseParties.ToCreate.ForExistingCase> casePartiesCreator
) : LocatableCreator<T>(
    inserters,
    nodeDetailsCreator,
    nameableDetailsCreator,
    locatableDetailsCreator
), IAsyncDisposable
    where T : class, CaseToCreate
{
    public override async Task ProcessAsync(T element, int id)
    {
        await base.ProcessAsync(element, id);
        await casePartiesCreator
            .CreateAsync(element.CaseDetails.CaseCaseParties
                .Select(x => x.ResolvedCase(id))
                .ToAsyncEnumerable());
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await casePartiesCreator.DisposeAsync();
    }
}
public class LocatableCreator<T>(
    List<IDatabaseInserter<T>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    TermCreator nameableDetailsCreator,
    LocatableDetailsCreator locatableDetailsCreator
) : NameableCreator<T>(
    inserters,
    nodeDetailsCreator,
    nameableDetailsCreator
), IAsyncDisposable
    where T : class, LocatableToCreate
{
    public override async Task ProcessAsync(T element, int id)
    {
        await base.ProcessAsync(element, id);
        await locatableDetailsCreator.Process(element);
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await locatableDetailsCreator.DisposeAsync();
    }
}
public class NameableCreator<T>(
    List<IDatabaseInserter<T>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    TermCreator termCreator
) : NodeCreator<T>(
    inserters,
    nodeDetailsCreator
), IAsyncDisposable
    where T : class, NameableToCreate
{
    public override async Task ProcessAsync(T element, int id)
    {
        await base.ProcessAsync(element, id);
        await termCreator
            .CreateAsync(element.NameableDetails.Terms
                .Select(x => x.ResolveNameable(id))
                .ToAsyncEnumerable());
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await termCreator.DisposeAsync();
    }
}
public class NodeDetailsCreatorFactory(
    IDatabaseInserterFactory<ResolvedNodeTermToAdd> nodeTermInserterFactory,
    IDatabaseInserterFactory<TenantNode.ToCreateForExistingNode> tenantNodeInserterFactory
)
{ 
    public async Task<NodeDetailsCreator> CreateAsync(IDbConnection connection)
    {
        return new NodeDetailsCreator(
            await nodeTermInserterFactory.CreateAsync(connection),
            await tenantNodeInserterFactory.CreateAsync(connection)
        );
    }
}

public class NodeDetailsCreator(
    IDatabaseInserter<ResolvedNodeTermToAdd> nodeTermInserter,
    IDatabaseInserter<TenantNode.ToCreateForExistingNode> tenantNodeInserter
) : IAsyncDisposable
{
    public async Task ProcessAsync(NodeToCreate element, int id)
    {
        foreach (var nodeTermId in element.NodeDetails.TermIds) {
            await nodeTermInserter.InsertAsync(new ResolvedNodeTermToAdd 
            { 
                NodeId = id, 
                TermId = nodeTermId 
            });
        }
        foreach(var tenantNode in element.NodeDetails.TenantNodes)
        { 
            await tenantNodeInserter.InsertAsync(tenantNode.ResolveNodeId(id));
        }
    }

    public async ValueTask DisposeAsync()
    {
        await nodeTermInserter.DisposeAsync();
        await tenantNodeInserter.DisposeAsync();
    }
}

public class NodeCreator<T>(
    List<IDatabaseInserter<T>> inserters,
    NodeDetailsCreator nodeDetailsCreator

) : InsertingEntityCreator<T>(inserters)
    where T: class, NodeToCreate
{
    public virtual async Task ProcessAsync(T element, int id)
    {
        await nodeDetailsCreator.ProcessAsync(element, id);
    }

    public sealed override async Task ProcessAsync(T element)
    {
        await base.ProcessAsync(element);
        await ProcessAsync(element, element.Identification.Id!.Value);
    }

    public override async ValueTask DisposeAsync()
    {
        await nodeDetailsCreator.DisposeAsync();
    }
}
public class InsertingEntityCreator<T>(
    List<IDatabaseInserter<T>> inserters
 ) : EntityCreator<T>, IAsyncDisposable
    where T : class, IRequest
{
    public override async Task ProcessAsync(T element)
    {
        foreach (var inserter in inserters) {
            await inserter.InsertAsync(element);
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        foreach (var elem in inserters) {
            await elem.DisposeAsync();
        }
    }
}

public class EntityCreator<T>() : IEntityCreator<T>, IAsyncDisposable
    where T : class, IRequest
{
    public virtual async Task ProcessAsync(T element)
    {
        await Task.CompletedTask;
    }

    public async Task CreateAsync(IAsyncEnumerable<T> elements)
    {
        await foreach (var element in elements) {
            await ProcessAsync(element);
        }
    }
    public async Task CreateAsync(T element)
    {
        await CreateAsync(new List<T> { element }.ToAsyncEnumerable());
    }
    public virtual async ValueTask DisposeAsync()
    {
        await Task.CompletedTask;
    }
}

