namespace PoundPupLegacy.Db;

public class CaseRelationTypeCreator : IEntityCreator<CasePartyType>
{
    public static async Task CreateAsync(IAsyncEnumerable<CasePartyType> caseRelationTypes, NpgsqlConnection connection)
    {

        await using var caseRelationTypeWriter = await CasePartyTypeWriter.CreateAsync(connection);

        await foreach (var caseRelationType in caseRelationTypes)
        {
            await caseRelationTypeWriter.WriteAsync(caseRelationType);
        }
    }
}
