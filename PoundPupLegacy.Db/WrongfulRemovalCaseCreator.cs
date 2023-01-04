﻿using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class WrongfulRemovalCaseCreator : IEntityCreator<WrongfulRemovalCase>
{
    public static async Task CreateAsync(IAsyncEnumerable<WrongfulRemovalCase> wrongfulRemovalCases, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var locatableWriter = await LocatableWriter.CreateAsync(connection);
        await using var caseWriter = await CaseWriter.CreateAsync(connection);
        await using var wrongfulRemovalCaseWriter = await WrongfulRemovalCaseWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        await foreach (var wrongfulRemovalCase in wrongfulRemovalCases)
        {
            await nodeWriter.WriteAsync(wrongfulRemovalCase);
            await documentableWriter.WriteAsync(wrongfulRemovalCase);
            await locatableWriter.WriteAsync(wrongfulRemovalCase);
            await caseWriter.WriteAsync(wrongfulRemovalCase);
            await wrongfulRemovalCaseWriter.WriteAsync(wrongfulRemovalCase);
            await EntityCreator.WriteTerms(wrongfulRemovalCase, termWriter, termReader, termHierarchyWriter);
        }
    }
}
