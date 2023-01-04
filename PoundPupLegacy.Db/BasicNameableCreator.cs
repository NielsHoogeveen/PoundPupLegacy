using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class BasicNameableCreator : IEntityCreator<BasicNameable>
{
    public static async Task CreateAsync(IAsyncEnumerable<BasicNameable> basicNameables, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var basicNameableWriter = await BasicNameableWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await foreach (var basicNameable in basicNameables)
        {
            await nodeWriter.WriteAsync(basicNameable);
            await nameableWriter.WriteAsync(basicNameable);
            await basicNameableWriter.WriteAsync(basicNameable);
            await EntityCreator.WriteTerms(basicNameable, termWriter, termReader, termHierarchyWriter);
        }
    }
}
