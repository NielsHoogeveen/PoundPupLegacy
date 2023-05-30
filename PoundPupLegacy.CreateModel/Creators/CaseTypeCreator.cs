namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CaseTypeCreatorFactory(
    IDatabaseInserterFactory<NodeTypeToAdd> nodeTypeInserterFactory,
    IDatabaseInserterFactory<CaseType> caseTypeInserterFactory,
    IDatabaseInserterFactory<NameableTypeToAdd> nameableTypeInserterFactory,
    IDatabaseInserterFactory<CaseTypeCasePartyType> caseTypeCasePartyTypeInserterFactory
) : IEntityCreatorFactory<CaseType>
{
    public async Task<IEntityCreator<CaseType>> CreateAsync(IDbConnection connection) =>
        new CaseTypeCreator(
            new() {
                await nodeTypeInserterFactory.CreateAsync(connection),
                await nameableTypeInserterFactory.CreateAsync(connection),
                await caseTypeInserterFactory.CreateAsync(connection)
                
            },
            await caseTypeCasePartyTypeInserterFactory.CreateAsync(connection)
        );
}

internal sealed class CaseTypeCreator(
    List<IDatabaseInserter<CaseType>> inserters,
    IDatabaseInserter<CaseTypeCasePartyType> caseTypeCasePartyTypeInserter
) : InsertingEntityCreator<CaseType>(inserters)
{
    public override async Task ProcessAsync(CaseType element)
    {
        await base.ProcessAsync(element);
        foreach (var caseRelationTypeId in element.CaseRelationTypeIds) {
            await caseTypeCasePartyTypeInserter.InsertAsync(new CaseTypeCasePartyType {
                CasePartyTypeId = caseRelationTypeId,
                CaseTypeId = element.IdentificationForCreate.Id!.Value
            });
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await caseTypeCasePartyTypeInserter.DisposeAsync();
    }
}