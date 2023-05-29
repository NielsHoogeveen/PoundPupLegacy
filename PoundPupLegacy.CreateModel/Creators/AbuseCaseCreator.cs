namespace PoundPupLegacy.CreateModel.Creators;

public class AbuseCaseCreatorFactory(

    IDatabaseInserterFactory<NodeToCreate> nodeInserterFactory,
    IDatabaseInserterFactory<SearchableToCreate> searchableInserterFactory,
    IDatabaseInserterFactory<DocumentableToCreate> documentableInserterFactory,
    IDatabaseInserterFactory<LocatableToCreate> locatableInserterFactory,
    IDatabaseInserterFactory<NameableToCreate> nameableInserterFactory,
    IDatabaseInserterFactory<CaseToCreate> caseInserterFactory,
    IDatabaseInserterFactory<AbuseCase.AbuseCaseToCreate> abuseCaseInserterFactory,
    LocatableDetailsCreatorFactory locatableDetailsCreatorFactory,
    TermCreatorFactory nameableDetailsCreatorFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    IEntityCreatorFactory<CaseNewCasePartiesToUpdate> caseCaseTypeCreatorFactory,
    IEntityCreatorFactory<AbuseCaseTypeOfAbuse> abuseCaseTypeOfAbuseCreatorFactory,
    IEntityCreatorFactory<AbuseCaseTypeOfAbuser> abuseCaseTypeOfAbuserCreatorFactory
) : IEntityCreatorFactory<AbuseCase.AbuseCaseToCreate>
{
    public async Task<IEntityCreator<AbuseCase.AbuseCaseToCreate>> CreateAsync(IDbConnection connection) => 
        new AbuseCaseCreator(
            new () {
                await nodeInserterFactory.CreateAsync(connection),
                await searchableInserterFactory.CreateAsync(connection),
                await documentableInserterFactory.CreateAsync(connection),
                await locatableInserterFactory.CreateAsync(connection),
                await nameableInserterFactory.CreateAsync(connection),
                await caseInserterFactory.CreateAsync(connection),
                await abuseCaseInserterFactory.CreateAsync(connection)
            },
            await nodeDetailsCreatorFactory.CreateAsync(connection),
            await nameableDetailsCreatorFactory.CreateAsync(connection),
            await locatableDetailsCreatorFactory.CreateAsync(connection),
            await caseCaseTypeCreatorFactory.CreateAsync(connection),
            await abuseCaseTypeOfAbuseCreatorFactory.CreateAsync(connection),
            await abuseCaseTypeOfAbuserCreatorFactory.CreateAsync(connection)
        );
}

public class AbuseCaseCreator(
    List<IDatabaseInserter<AbuseCase.AbuseCaseToCreate>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    TermCreator nameableDetailsCreator,
    LocatableDetailsCreator locatableDetailsCreator,
    IEntityCreator<CaseNewCasePartiesToUpdate> caseCaseTypeCreator,
    IEntityCreator<AbuseCaseTypeOfAbuse> abuseCaseTypeOfAbuseCreator,
    IEntityCreator<AbuseCaseTypeOfAbuser> abuseCaseTypeOfAbuserCreator
) : CaseCreator<AbuseCase.AbuseCaseToCreate>
(
    inserters,
    nodeDetailsCreator,
    nameableDetailsCreator,
    locatableDetailsCreator,
    caseCaseTypeCreator
)
{
    public override async Task ProcessAsync(AbuseCase.AbuseCaseToCreate element, int id)
    {
        await base.ProcessAsync(element, id);
        foreach (var typeOfAbuseId in element.AbuseCaseDetails.TypeOfAbuseIds) {
            await abuseCaseTypeOfAbuseCreator.CreateAsync(
                new AbuseCaseTypeOfAbuse {
                    AbuseCaseId = id,
                    TypeOfAbuseId = typeOfAbuseId
                }
            );
        }
        foreach (var typeOfAbuserId in element.AbuseCaseDetails.TypeOfAbuserIds) {
            await abuseCaseTypeOfAbuserCreator.CreateAsync(
                new AbuseCaseTypeOfAbuser {
                    AbuseCaseId = id,
                    TypeOfAbuserId = typeOfAbuserId
                }
            );
        }
    }
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync();
        await abuseCaseTypeOfAbuseCreator.DisposeAsync();
        await abuseCaseTypeOfAbuserCreator.DisposeAsync();
    }
}