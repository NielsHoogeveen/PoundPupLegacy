using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class TermHierarchyCreatorFactory(
    IDatabaseInserterFactory<TermHierarchy> termHierarchyInserterFactory
) : IEntityCreatorFactory<TermHierarchy>
{
    public async Task<IEntityCreator<TermHierarchy>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<TermHierarchy>(new() {
            await termHierarchyInserterFactory.CreateAsync(connection)
        });
}
