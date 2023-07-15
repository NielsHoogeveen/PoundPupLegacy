using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class VocabularyCreatorFactory(
    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<Vocabulary.ToCreate> vocabularyInserterFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory
) : IEntityCreatorFactory<Vocabulary.ToCreate>
{
    public async Task<IEntityCreator<Vocabulary.ToCreate>> CreateAsync(IDbConnection connection) =>
        new NodeCreator<Vocabulary.ToCreate>(
            new() {
                await nodeInserterFactory.CreateAsync(connection),
                await vocabularyInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection)
        );
}
