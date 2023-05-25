namespace PoundPupLegacy.CreateModel.Creators;
internal sealed class AbuseCaseTypeOfAbuseCreatorFactory(
    IDatabaseInserterFactory<AbuseCaseTypeOfAbuse> abuseCaseTypeOfAbuseInserterFactory
) : IEntityCreatorFactory<AbuseCaseTypeOfAbuse>
{
    public async Task<IEntityCreator<AbuseCaseTypeOfAbuse>> CreateAsync(IDbConnection connection) => 
        new InsertingEntityCreator<AbuseCaseTypeOfAbuse>(new () {
            await abuseCaseTypeOfAbuseInserterFactory.CreateAsync(connection)
        });
}
