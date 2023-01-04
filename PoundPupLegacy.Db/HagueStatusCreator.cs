using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class HagueStatusCreator : IEntityCreator<HagueStatus>
{
    public static async Task CreateAsync(IEnumerable<HagueStatus> hagueStatuss, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var hagueStatusWriter = await HagueStatusWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        foreach (var hagueStatus in hagueStatuss)
        {
            await nodeWriter.WriteAsync(hagueStatus);
            await nameableWriter.WriteAsync(hagueStatus);
            await hagueStatusWriter.WriteAsync(hagueStatus);
            await EntityCreator.WriteTerms(hagueStatus, termWriter, termReader, termHierarchyWriter);
        }
    }
}
