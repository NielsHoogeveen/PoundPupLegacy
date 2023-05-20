namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CollectiveCreator(
    IDatabaseInserterFactory<Principal> principalInserterFactory,
    IDatabaseInserterFactory<AccessRole> accessRoleInserterFactory,
    IDatabaseInserterFactory<Publisher> publisherInserterFactory,
    IDatabaseInserterFactory<Collective> collectiveInserterFactory
) : EntityCreator<Collective>
{
    public override async Task CreateAsync(IAsyncEnumerable<Collective> collectives, IDbConnection connection)
    {
        await using var principalWriter = await principalInserterFactory.CreateAsync(connection);
        await using var accessRoleWriter = await accessRoleInserterFactory.CreateAsync(connection);
        await using var publisherWriter = await publisherInserterFactory.CreateAsync(connection);
        await using var userWriter = await collectiveInserterFactory.CreateAsync(connection);

        await foreach (var collective in collectives) {
            await principalWriter.InsertAsync(collective);
            await publisherWriter.InsertAsync(collective);
            await userWriter.InsertAsync(collective);
        }
    }
}