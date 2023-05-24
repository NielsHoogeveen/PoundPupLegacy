namespace PoundPupLegacy.CreateModel.Creators;

public interface IEntityCreator<T>: IAsyncDisposable
{
    Task CreateAsync(IAsyncEnumerable<T> elements);
    Task CreateAsync(T element);
}

public interface INameableCreatorFactory<T> : IEntityCreatorFactory<T, NameableCreator<T>>
    where T : class, EventuallyIdentifiableNameable
{
}

public interface INodeCreatorFactory<T>: IEntityCreatorFactory<T, NodeCreator<T>>
    where T : class, EventuallyIdentifiableNode
{
}
public interface IInsertingEntityCreatorFactory<T> : IEntityCreatorFactory<T, InsertingEntityCreator<T>>
    where T : class, IRequest
{
}

public interface IEntityCreatorFactory<T, TEntityCreator>
    where T: IRequest
    where TEntityCreator: IEntityCreator<T>
{
    Task<TEntityCreator> CreateAsync(IDbConnection connection);
}
public interface IEntityCreatorFactory<T>: IEntityCreatorFactory<T, EntityCreator<T>>
    where T : class, IRequest
{
}

public class NameableDetailsCreatorFactory(
    IDatabaseInserterFactory<Term> termInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<TermReaderByNameRequest, Term> termReaderFactory,
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory,
    IMandatorySingleItemDatabaseReaderFactory<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReaderFactory
)
{
    public async Task<NameableDetailsCreator> CreateAsync(IDbConnection connection)
    {
        return new NameableDetailsCreator(
            await termInserterFactory.CreateAsync(connection),
            await termReaderFactory.CreateAsync(connection),
            await termHierarchyInserterFactory.CreateAsync(connection),
            await vocabularyIdReaderFactory.CreateAsync(connection)
        );
    }
}

public class NameableDetailsCreator(
    IDatabaseInserter<Term> termInserter,
    IMandatorySingleItemDatabaseReader<TermReaderByNameRequest, Term> termReader,
    IDatabaseInserter<TermHierarchy> termHierarchyInserter,
    IMandatorySingleItemDatabaseReader<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReader
) : IAsyncDisposable
{
    public async Task Process(EventuallyIdentifiableNameable nameable)
    {
        foreach (var vocabularyName in nameable.VocabularyNames) {
            var vocubularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndNameRequest {
                OwnerId = vocabularyName.OwnerId,
                Name = vocabularyName.Name
            });
            var term = new Term {
                Name = vocabularyName.TermName,
                Id = null,
                VocabularyId = vocubularyId,
                NameableId = (int)nameable.Id!
            };
            await termInserter.InsertAsync(term);
            foreach (var parent in vocabularyName.ParentNames) {
                var parentTerm = await termReader.ReadAsync(new TermReaderByNameRequest {
                    Name = parent,
                    VocabularyId = vocubularyId
                });
                await termHierarchyInserter.InsertAsync(new TermHierarchy { TermIdPartent = parentTerm.Id!.Value, TermIdChild = (int)term.Id! });
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        await termInserter.DisposeAsync();
        await termHierarchyInserter.DisposeAsync();
        await termReader.DisposeAsync();
        await vocabularyIdReader.DisposeAsync();
    }
}

public class NameableCreator<T>(
    List<IDatabaseInserter<T>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    NameableDetailsCreator nameableDetailsCreator
) : NodeCreator<T>(
    inserters,
    nodeDetailsCreator
), IAsyncDisposable
    where T : class, EventuallyIdentifiableNameable
{
    public override async Task ProcessAsync(T element)
    {
        await base.ProcessAsync(element);
        await nameableDetailsCreator.Process(element);
    }

    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await nameableDetailsCreator.DisposeAsync();
    }
}
public class NodeDetailsCreatorFactory(
    IDatabaseInserterFactory<NodeTerm> nodeTermInserterFactory,
    IDatabaseInserterFactory<NewTenantNodeForNewNode> tenantNodeInserterFactory
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
    IDatabaseInserter<NodeTerm> nodeTermInserter,
    IDatabaseInserter<NewTenantNodeForNewNode> tenantNodeInserter
) : IAsyncDisposable
{
    public async Task ProcessAsync(EventuallyIdentifiableNode element)
    {
        foreach (var nodeTermId in element.NodeTermIds) {
            var nodeTerm = new NodeTerm { NodeId = element.Id!.Value, TermId = nodeTermId };
            await nodeTermInserter.InsertAsync(nodeTerm);
        }
        foreach (var tenantNode in element.TenantNodes) {
            tenantNode.NodeId = element.Id;
            await tenantNodeInserter.InsertAsync(tenantNode);
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
    where T: class, EventuallyIdentifiableNode
{

    
    public override async Task ProcessAsync(T element)
    {
        await base.ProcessAsync(element);
        await nodeDetailsCreator.ProcessAsync(element);
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

//public interface IEntityCreatorDeprecated<T>
//{
//    Task CreateAsync(IAsyncEnumerable<T> elements, IDbConnection connection);
//    Task CreateAsync(T element, IDbConnection connection);
//}

//internal abstract class EntityCreatorDeprecated<T> : IEntityCreatorDeprecated<T>
//{
//    public abstract Task CreateAsync(IAsyncEnumerable<T> elements, IDbConnection connection);
//    public async Task CreateAsync(T element, IDbConnection connection)
//    {
//        await CreateAsync(new List<T> { element }.ToAsyncEnumerable(), connection);
//    }

//    internal static async Task WriteTerms(EventuallyIdentifiableNameable nameable, IDatabaseInserter<Term> termWriter, IMandatorySingleItemDatabaseReader<TermReaderByNameRequest, Term> termReader, IDatabaseInserter<TermHierarchy> termHierarchyWriter, IMandatorySingleItemDatabaseReader<VocabularyIdReaderByOwnerAndNameRequest, int> vocabularyIdReader)
//    {
//        foreach (var vocabularyName in nameable.VocabularyNames) {
//            var vocubularyId = await vocabularyIdReader.ReadAsync(new VocabularyIdReaderByOwnerAndNameRequest {
//                OwnerId = vocabularyName.OwnerId,
//                Name = vocabularyName.Name
//            });
//            var term = new Term {
//                Name = vocabularyName.TermName,
//                Id = null,
//                VocabularyId = vocubularyId,
//                NameableId = (int)nameable.Id!
//            };
//            await termWriter.InsertAsync(term);
//            foreach (var parent in vocabularyName.ParentNames) {
//                var parentTerm = await termReader.ReadAsync(new TermReaderByNameRequest {
//                    Name = parent,
//                    VocabularyId = vocubularyId
//                });
//                await termHierarchyWriter.InsertAsync(new TermHierarchy { TermIdPartent = parentTerm.Id!.Value, TermIdChild = (int)term.Id! });
//            }
//        }
//    }
//}