using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class TypeOfAbuseCreator : IEntityCreator<TypeOfAbuse>
{
    public static async Task CreateAsync(IEnumerable<TypeOfAbuse> typesOfAbuse, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var typeOfAbuseWriter = await TypeOfAbuseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        foreach (var typeOfAbuse in typesOfAbuse)
        {
            await nodeWriter.WriteAsync(typeOfAbuse);
            await nameableWriter.WriteAsync(typeOfAbuse);
            await typeOfAbuseWriter.WriteAsync(typeOfAbuse);
            await EntityCreator.WriteTerms(typeOfAbuse, termWriter, termReader, termHierarchyWriter);
        }
    }
}
