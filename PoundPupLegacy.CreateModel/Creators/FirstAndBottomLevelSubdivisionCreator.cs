﻿namespace PoundPupLegacy.CreateModel.Creators;

public class FirstAndBottomLevelSubdivisionCreator : IEntityCreator<FirstAndBottomLevelSubdivision>
{
    public static async Task CreateAsync(IAsyncEnumerable<FirstAndBottomLevelSubdivision> subdivisions, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeInserter.CreateAsync(connection);
        await using var searchableWriter = await SearchableInserter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableInserter.CreateAsync(connection);
        await using var nameableWriter = await NameableInserter.CreateAsync(connection);
        await using var geographicalEntityWriter = await GeographicalEnityInserter.CreateAsync(connection);
        await using var politicalEntityWriter = await PoliticalEntityInserter.CreateAsync(connection);
        await using var subdivisionWriter = await SubdivisionInserter.CreateAsync(connection);
        await using var isoCodedSubdivisionWriter = await ISOCodedSubdivisionInserter.CreateAsync(connection);
        await using var firstLevelSubdivisionWriter = await FirstLevelSubdivisionInserter.CreateAsync(connection);
        await using var isoCodedFirstLevelSubdivisionWriter = await ISOCodedFirstLevelSubdivisionInserter.CreateAsync(connection);
        await using var bottomLevelSubdivisionWriter = await BottomLevelSubdivisionInserter.CreateAsync(connection);
        await using var firstAndBottomLevelSubdivisionWriter = await FirstAndBottomLevelSubdivisionInserter.CreateAsync(connection);
        await using var termWriter = await TermInserter.CreateAsync(connection);
        await using var termReader = await new TermReaderByNameFactory().CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyInserter.CreateAsync(connection);
        await using var vocabularyIdReader = await new VocabularyIdReaderByOwnerAndNameFactory().CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeInserter.CreateAsync(connection);

        await foreach (var subdivision in subdivisions) {
            await nodeWriter.WriteAsync(subdivision);
            await searchableWriter.WriteAsync(subdivision);
            await documentableWriter.WriteAsync(subdivision);
            await nameableWriter.WriteAsync(subdivision);
            await geographicalEntityWriter.WriteAsync(subdivision);
            await politicalEntityWriter.WriteAsync(subdivision);
            await subdivisionWriter.WriteAsync(subdivision);
            await bottomLevelSubdivisionWriter.WriteAsync(subdivision);
            await isoCodedSubdivisionWriter.WriteAsync(subdivision);
            await firstLevelSubdivisionWriter.WriteAsync(subdivision);
            await isoCodedFirstLevelSubdivisionWriter.WriteAsync(subdivision);
            await firstAndBottomLevelSubdivisionWriter.WriteAsync(subdivision);
            await EntityCreator.WriteTerms(subdivision, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in subdivision.TenantNodes) {
                tenantNode.NodeId = subdivision.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
