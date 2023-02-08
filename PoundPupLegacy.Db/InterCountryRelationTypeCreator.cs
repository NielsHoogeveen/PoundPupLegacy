using PoundPupLegacy.Db.Readers;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db;

public class InterCountryRelationTypeCreator : IEntityCreator<InterCountryRelationType>
{
    public static async Task CreateAsync(IAsyncEnumerable<InterCountryRelationType> interCountryRelationTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var interCountryRelationTypeWriter = await InterCountryRelationTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReaderByName.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await VocabularyIdReaderByOwnerAndName.CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var interCountryRelationType in interCountryRelationTypes)
        {
            await nodeWriter.WriteAsync(interCountryRelationType);
            await searchableWriter.WriteAsync(interCountryRelationType);
            await nameableWriter.WriteAsync(interCountryRelationType);
            await interCountryRelationTypeWriter.WriteAsync(interCountryRelationType);
            await EntityCreator.WriteTerms(interCountryRelationType, termWriter, termReader, termHierarchyWriter,vocabularyIdReader);
            foreach (var tenantNode in interCountryRelationType.TenantNodes)
            {
                tenantNode.NodeId = interCountryRelationType.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
