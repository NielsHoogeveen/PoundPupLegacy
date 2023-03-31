﻿namespace PoundPupLegacy.CreateModel.Creators;

public class NodeTypeCreator : IEntityCreator<BasicNodeType>
{
    public static async Task CreateAsync(IAsyncEnumerable<BasicNodeType> nodeTypes, NpgsqlConnection connection)
    {

        await using var nodeTypeWriter = await NodeTypeInserter.CreateAsync(connection);

        await foreach (var nodeType in nodeTypes) {
            await nodeTypeWriter.InsertAsync(nodeType);
        }
    }
}