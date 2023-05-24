namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AuthoringStatusCreatorFactory(
    IDatabaseInserterFactory<AuthoringStatus> authoringStatusInserterFactory
) : IInsertingEntityCreatorFactory<AuthoringStatus>
{
    public async Task<InsertingEntityCreator<AuthoringStatus>> CreateAsync(IDbConnection connection) =>
        new (new () {
            await authoringStatusInserterFactory.CreateAsync(connection)
        });
}
