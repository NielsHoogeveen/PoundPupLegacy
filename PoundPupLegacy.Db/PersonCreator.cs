﻿using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class PersonCreator : IEntityCreator<BasicPerson>
{
    public static async Task CreateAsync(IAsyncEnumerable<BasicPerson> persons, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var partyWriter = await PartyWriter.CreateAsync(connection);
        await using var personWriter = await PersonWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        await foreach (var person in persons)
        {
            await nodeWriter.WriteAsync(person);
            await documentableWriter.WriteAsync(person);
            await locatableWriter.WriteAsync(person);
            await partyWriter.WriteAsync(person);
            await personWriter.WriteAsync(person);
            await EntityCreator.WriteTerms(person, termWriter, termReader, termHierarchyWriter);
        }
    }
}
