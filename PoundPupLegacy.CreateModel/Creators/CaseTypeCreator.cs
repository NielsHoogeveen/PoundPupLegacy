namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CaseTypeCreator : IEntityCreator<CaseType>
{
    public async Task CreateAsync(IAsyncEnumerable<CaseType> caseTypes, IDbConnection connection)
    {

        await using var nodeTypeWriter = await NodeTypeInserter.CreateAsync(connection);
        await using var caseTypeWriter = await CaseTypeInserter.CreateAsync(connection);
        await using var caseTypeCaseRelationTypeWriter = await CaseTypeCasePartyTypeInserter.CreateAsync(connection);

        await foreach (var caseType in caseTypes) {
            await nodeTypeWriter.InsertAsync(caseType);
            await caseTypeWriter.InsertAsync(caseType);
            foreach (var caseRelationTypeId in caseType.CaseRelationTypeIds) {
                await caseTypeCaseRelationTypeWriter.InsertAsync(new CaseTypeCasePartyType {
                    CasePartyTypeId = caseRelationTypeId,
                    CaseTypeId = caseType.Id!.Value
                });
            }
        }
    }
}
