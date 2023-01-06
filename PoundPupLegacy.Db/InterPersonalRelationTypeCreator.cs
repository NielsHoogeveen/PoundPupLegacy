using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class InterPersonalRelationTypeCreator : IEntityCreator<InterPersonalRelationType>
{
    public static async Task CreateAsync(IAsyncEnumerable<InterPersonalRelationType> interPersonalRelationTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var interPersonalRelationTypeWriter = await InterPersonalRelationTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);


        await foreach (var interPersonalRelationType in interPersonalRelationTypes)
        {
            await nodeWriter.WriteAsync(interPersonalRelationType);
            await nameableWriter.WriteAsync(interPersonalRelationType);
            await interPersonalRelationTypeWriter.WriteAsync(interPersonalRelationType);
            await EntityCreator.WriteTerms(interPersonalRelationType, termWriter, termReader, termHierarchyWriter);
        }
    }
}
