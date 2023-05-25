namespace PoundPupLegacy.CreateModel.Creators;

public class AbuseCaseCreatorFactory(

    IDatabaseInserterFactory<EventuallyIdentifiableNode> nodeInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableSearchable> searchableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableDocumentable> documentableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableLocatable> locatableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableNameable> nameableInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableCase> caseInserterFactory,
    IDatabaseInserterFactory<EventuallyIdentifiableAbuseCase> abuseCaseInserterFactory,
    NameableDetailsCreatorFactory nameableDetailsCreatorFactory,
    NodeDetailsCreatorFactory nodeDetailsCreatorFactory,
    IEntityCreatorFactory<AbuseCaseTypeOfAbuse> abuseCaseTypeOfAbuseCreatorFactory,
    IEntityCreatorFactory<AbuseCaseTypeOfAbuser> abuseCaseTypeOfAbuserCreatorFactory
) : IEntityCreatorFactory<EventuallyIdentifiableAbuseCase>
{
    public async Task<IEntityCreator<EventuallyIdentifiableAbuseCase>> CreateAsync(IDbConnection connection) => 
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
            await abuseCaseTypeOfAbuseCreatorFactory.CreateAsync(connection),
            await abuseCaseTypeOfAbuserCreatorFactory.CreateAsync(connection)
        );
}

public class AbuseCaseCreator(
    List<IDatabaseInserter<EventuallyIdentifiableAbuseCase>> inserters,
    NodeDetailsCreator nodeDetailsCreator,
    NameableDetailsCreator nameableDetailsCreator,
    IEntityCreator<AbuseCaseTypeOfAbuse> abuseCaseTypeOfAbuseCreator,
    IEntityCreator<AbuseCaseTypeOfAbuser> abuseCaseTypeOfAbuserCreator

) : NameableCreator<EventuallyIdentifiableAbuseCase>
(
    inserters,
    nodeDetailsCreator,
    nameableDetailsCreator
)
{
    public override async Task ProcessAsync(EventuallyIdentifiableAbuseCase element)
    {
        await base.ProcessAsync(element);
        foreach (var typeOfAbuseId in element.TypeOfAbuseIds) {
            await abuseCaseTypeOfAbuseCreator.CreateAsync(
                new AbuseCaseTypeOfAbuse {
                    AbuseCaseId = element.Id!.Value,
                    TypeOfAbuseId = typeOfAbuseId
                }
            );
        }
        foreach (var typeOfAbuserId in element.TypeOfAbuserIds) {
            await abuseCaseTypeOfAbuserCreator.CreateAsync(
                new AbuseCaseTypeOfAbuser {
                    AbuseCaseId = element.Id!.Value,
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