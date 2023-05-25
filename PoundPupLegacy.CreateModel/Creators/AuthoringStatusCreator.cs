namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AuthoringStatusCreatorFactory(
    IDatabaseInserterFactory<AuthoringStatus> authoringStatusInserterFactory
) : IEntityCreatorFactory<AuthoringStatus>
{
    public async Task<IEntityCreator<AuthoringStatus>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<AuthoringStatus>(new () {
            await authoringStatusInserterFactory.CreateAsync(connection)
        });
}
