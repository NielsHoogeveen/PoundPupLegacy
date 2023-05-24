namespace PoundPupLegacy.CreateModel.Creators;
internal sealed class AbuseCaseTypeOfAbuseCreatorFactory(
    IDatabaseInserterFactory<AbuseCaseTypeOfAbuse> abuseCaseTypeOfAbuseInserterFactory
) : IInsertingEntityCreatorFactory<AbuseCaseTypeOfAbuse>
{
    public async Task<InsertingEntityCreator<AbuseCaseTypeOfAbuse>> CreateAsync(IDbConnection connection) => 
        new (new () {
            await abuseCaseTypeOfAbuseInserterFactory.CreateAsync(connection)
        });
}
