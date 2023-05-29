namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class VocabularyCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<Vocabulary.VocabularyToCreate> vocabularyInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<Vocabulary.VocabularyToCreate>
{
    public async Task<IEntityCreator<Vocabulary.VocabularyToCreate>> CreateAsync(IDbConnection connection)=>
        new NodeCreator<Vocabulary.VocabularyToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await vocabularyInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
