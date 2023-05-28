namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class VocabularyCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<VocabularyToCreate> vocabularyInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<VocabularyToCreate>
{
    public async Task<IEntityCreator<VocabularyToCreate>> CreateAsync(IDbConnection connection)=>
        new NodeCreator<VocabularyToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await vocabularyInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
