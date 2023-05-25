namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class VocabularyCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableVocabulary> vocabularyInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableVocabulary>
{
    public async Task<IEntityCreator<EventuallyIdentifiableVocabulary>> CreateAsync(IDbConnection connection)=>
        new NodeCreator<EventuallyIdentifiableVocabulary>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await vocabularyInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
