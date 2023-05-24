namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class PublicationStatusCreatorFactory(
    IDatabaseInserterFactory<PublicationStatus> publicationStatusInserterFactory
) : IInsertingEntityCreatorFactory<PublicationStatus>
{
    public async Task<InsertingEntityCreator<PublicationStatus>> CreateAsync(IDbConnection connection) => 
        new(new() {
            await publicationStatusInserterFactory.CreateAsync(connection)
        });
}
