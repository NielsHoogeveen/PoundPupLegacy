namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CaseTypeCreator(
    IDatabaseInserterFactory<NodeType> nodeTypeInserterFactory,
    IDatabaseInserterFactory<CaseType> caseTypeInserterFactory,
    IDatabaseInserterFactory<NameableType> nameableTypeInserterFactory,
    IDatabaseInserterFactory<CaseTypeCasePartyType> caseTypeCasePartyTypeInserterFactory
) : EntityCreator<CaseType>
{
    public override async Task CreateAsync(IAsyncEnumerable<CaseType> caseTypes, IDbConnection connection)
    {
        await using var nodeTypeWriter = await nodeTypeInserterFactory.CreateAsync(connection);
        await using var nameableTypeWriter = await nameableTypeInserterFactory.CreateAsync(connection);
        await using var caseTypeWriter = await caseTypeInserterFactory.CreateAsync(connection);
        await using var caseTypeCaseRelationTypeWriter = await caseTypeCasePartyTypeInserterFactory.CreateAsync(connection);

        await foreach (var caseType in caseTypes) {
            await nodeTypeWriter.InsertAsync(caseType);
            await nameableTypeWriter.InsertAsync(caseType);
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
