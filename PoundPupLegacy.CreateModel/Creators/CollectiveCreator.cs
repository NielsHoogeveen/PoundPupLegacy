namespace PoundPupLegacy.CreateModel.Creators;

public class CollectiveCreator : IEntityCreator<Collective>
{
    public async Task CreateAsync(IAsyncEnumerable<Collective> collectives, IDbConnection connection)
    {

        await using var principalWriter = await PrincipalInserter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleInserter.CreateAsync(connection);
        await using var publisherWriter = await PublisherInserter.CreateAsync(connection);
        await using var userWriter = await CollectiveInserter.CreateAsync(connection);

        await foreach (var collective in collectives) {
            await principalWriter.InsertAsync(collective);
            await publisherWriter.InsertAsync(collective);
            await userWriter.InsertAsync(collective);
        }
    }
}