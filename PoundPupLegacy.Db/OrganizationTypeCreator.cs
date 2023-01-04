using PoundPupLegacy.Db.Readers;

namespace PoundPupLegacy.Db;

public class OrganizationTypeCreator : IEntityCreator<OrganizationType>
{
    public static async Task CreateAsync(IAsyncEnumerable<OrganizationType> organizationTypes, NpgsqlConnection connection)
    {

        await using var nodeWriter = await NodeWriter.CreateAsync(connection);
        await using var nameableWriter = await NameableWriter.CreateAsync(connection);
        await using var organizationTypeWriter = await OrganizationTypeWriter.CreateAsync(connection);
        await using var termWriter = await TermWriter.CreateAsync(connection);
        await using var termReader = await TermReader.CreateAsync(connection);
        await using var termHierarchyWriter = await TermHierarchyWriter.CreateAsync(connection);

        await foreach (var organizationType in organizationTypes)
        {
            await nodeWriter.WriteAsync(organizationType);
            await nameableWriter.WriteAsync(organizationType);
            await organizationTypeWriter.WriteAsync(organizationType);
            await EntityCreator.WriteTerms(organizationType, termWriter, termReader, termHierarchyWriter);
        }
    }
}
