namespace PoundPupLegacy.Db;

public class CollectiveCreator : IEntityCreator<Collective>
{
    public static async Task CreateAsync(IAsyncEnumerable<Collective> collectives, NpgsqlConnection connection)
    {

        await using var principalWriter = await PrincipalWriter.CreateAsync(connection);
        await using var accessRoleWriter = await AccessRoleWriter.CreateAsync(connection);
        await using var publisherWriter = await PublisherWriter.CreateAsync(connection);
        await using var userWriter = await CollectiveWriter.CreateAsync(connection);

        await foreach (var collective in collectives) {
            await principalWriter.WriteAsync(collective);
            await publisherWriter.WriteAsync(collective);
            await userWriter.WriteAsync(collective);
        }
    }
}