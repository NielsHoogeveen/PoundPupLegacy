namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class PublicationStatusCreatorFactory(
    IDatabaseInserterFactory<PublicationStatus> publicationStatusInserterFactory
) : IEntityCreatorFactory<PublicationStatus>
{
    public async Task<IEntityCreator<PublicationStatus>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<PublicationStatus>(new() {
            await publicationStatusInserterFactory.CreateAsync(connection)
        });
}
