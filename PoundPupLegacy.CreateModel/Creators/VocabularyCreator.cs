namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class VocabularyCreatorFactory(
    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableVocabulary> vocabularyInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : INodeCreatorFactory<EventuallyIdentifiableVocabulary>
{
    public async Task<NodeCreator<EventuallyIdentifiableVocabulary>> CreateAsync(IDbConnection connection)=>
        new(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await vocabularyInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
