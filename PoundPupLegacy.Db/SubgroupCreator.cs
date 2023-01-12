namespace PoundPupLegacy.Db;

public class SubgroupCreator : IEntityCreator<Subgroup>
{
    public static async Task CreateAsync(IAsyncEnumerable<Subgroup> subgroups, NpgsqlConnection connection)
    {

        await using var userGroupWriter = await UserGroupWriter.CreateAsync(connection);
        await using var subgroupWriter = await SubgroupWriter.CreateAsync(connection);

        await foreach (var subgroup in subgroups)
        {
            await userGroupWriter.WriteAsync(subgroup);
            await subgroupWriter.WriteAsync(subgroup);
        }
    }
}
