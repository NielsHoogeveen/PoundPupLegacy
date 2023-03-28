using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class UnitedStatesCongressionalMeetingCreator : IEntityCreator<UnitedStatesCongressionalMeeting>
{
    public static async Task CreateAsync(IAsyncEnumerable<UnitedStatesCongressionalMeeting> countries, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var searchableWriter = await SearchableWriter.CreateAsync(connection);
        await using var documentableWriter = await DocumentableWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var unitedStatesCongressionalMeetingWriter = await UnitedStatesCongressionalMeetingWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await (new TermReaderByNameFactory()).CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);
        await using var vocabularyIdReader = await (new VocabularyIdReaderByOwnerAndNameFactory()).CreateAsync(connection);
        await using var tenantNodeWriter = await TenantNodeWriter.CreateAsync(connection);

        await foreach (var country in countries) {
            await nodeWriter.WriteAsync(country);
            await searchableWriter.WriteAsync(country);
            await documentableWriter.WriteAsync(country);
            await nameableWriter.WriteAsync(country);
            await unitedStatesCongressionalMeetingWriter.WriteAsync(country);
            await EntityCreator.WriteTerms(country, termWriter, termReader, termHierarchyWriter, vocabularyIdReader);
            foreach (var tenantNode in country.TenantNodes) {
                tenantNode.NodeId = country.Id;
                await tenantNodeWriter.WriteAsync(tenantNode);
            }

        }
    }
}
