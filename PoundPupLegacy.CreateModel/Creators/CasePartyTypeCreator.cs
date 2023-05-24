namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CasePartyTypeCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableCasePartyType> casePartyTypeInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory
) : INameableCreatorFactory<EventuallyIdentifiableCasePartyType>
{
    public async Task<NameableCreator<EventuallyIdentifiableCasePartyType>> CreateAsync(IDbConnection connection) =>
        new (
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await casePartyTypeInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection)
        );
}
