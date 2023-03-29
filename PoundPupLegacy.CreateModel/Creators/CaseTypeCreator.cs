namespace PoundPupLegacy.CreateModel.Creators;

public class CaseTypeCreator : IEntityCreator<CaseType>
{
    public static async Task CreateAsync(IAsyncEnumerable<CaseType> caseTypes, NpgsqlConnection connection)
    {

        await using var nodeTypeWriter = await NodeTypeWriter.CreateAsync(connection);
        await using var caseTypeWriter = await CaseTypeWriter.CreateAsync(connection);
        await using var caseTypeCaseRelationTypeWriter = await CaseTypeCasePartyTypeWriter.CreateAsync(connection);

        await foreach (var caseType in caseTypes) {
            await nodeTypeWriter.WriteAsync(caseType);
            await caseTypeWriter.WriteAsync(caseType);
            foreach (var caseRelationTypeId in caseType.CaseRelationTypeIds) {
                await caseTypeCaseRelationTypeWriter.WriteAsync(new CaseTypeCasePartyType {
                    CasePartyTypeId = caseRelationTypeId,
                    CaseTypeId = caseType.Id!.Value
                });
            }
        }
    }
}
