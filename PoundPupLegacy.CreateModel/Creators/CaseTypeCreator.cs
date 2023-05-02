namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CaseTypeCreator : EntityCreator<CaseType>
{
    private readonly IDatabaseInserterFactory<NodeType> _nodeTypeInserterFactory;
    private readonly IDatabaseInserterFactory<NameableType> _nameableTypeInserterFactory;
    private readonly IDatabaseInserterFactory<CaseType> _caseTypeInserterFactory;
    private readonly IDatabaseInserterFactory<CaseTypeCasePartyType> _caseTypeCasePartyTypeInserterFactory;
    public CaseTypeCreator(
        IDatabaseInserterFactory<NodeType> nodeTypeInserterFactory,
        IDatabaseInserterFactory<CaseType> caseTypeInserterFactory,
        IDatabaseInserterFactory<NameableType> nameableTypeInserterFactory,
        IDatabaseInserterFactory<CaseTypeCasePartyType> caseTypeCasePartyTypeInserterFactory
        )
    {
        _nodeTypeInserterFactory = nodeTypeInserterFactory;
        _caseTypeInserterFactory = caseTypeInserterFactory;
        _caseTypeCasePartyTypeInserterFactory = caseTypeCasePartyTypeInserterFactory;
        _nameableTypeInserterFactory = nameableTypeInserterFactory;

    }
    public override async Task CreateAsync(IAsyncEnumerable<CaseType> caseTypes, IDbConnection connection)
    {

        await using var nodeTypeWriter = await _nodeTypeInserterFactory.CreateAsync(connection);
        await using var nameableTypeWriter = await _nameableTypeInserterFactory.CreateAsync(connection);
        await using var caseTypeWriter = await _caseTypeInserterFactory.CreateAsync(connection);
        await using var caseTypeCaseRelationTypeWriter = await _caseTypeCasePartyTypeInserterFactory.CreateAsync(connection);

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
