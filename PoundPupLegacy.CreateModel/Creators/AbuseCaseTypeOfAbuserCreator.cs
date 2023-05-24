namespace PoundPupLegacy.CreateModel.Creators;
internal sealed class AbuseCaseTypeOfAbuserCreatorFactory(
    IDatabaseInserterFactory<AbuseCaseTypeOfAbuser> AbuseCaseTypeOfAbuserInserterFactory
) : IInsertingEntityCreatorFactory<AbuseCaseTypeOfAbuser>
{
    public async Task<InsertingEntityCreator<AbuseCaseTypeOfAbuser>> CreateAsync(IDbConnection connection) =>
        new (new (){
            await AbuseCaseTypeOfAbuserInserterFactory.CreateAsync(connection),
        });
}
