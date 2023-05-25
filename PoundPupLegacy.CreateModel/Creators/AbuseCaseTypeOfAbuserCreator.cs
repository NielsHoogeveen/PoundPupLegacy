namespace PoundPupLegacy.CreateModel.Creators;
internal sealed class AbuseCaseTypeOfAbuserCreatorFactory(
    IDatabaseInserterFactory<AbuseCaseTypeOfAbuser> AbuseCaseTypeOfAbuserInserterFactory
) : IEntityCreatorFactory<AbuseCaseTypeOfAbuser>
{
    public async Task<IEntityCreator<AbuseCaseTypeOfAbuser>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<AbuseCaseTypeOfAbuser>(new (){
            await AbuseCaseTypeOfAbuserInserterFactory.CreateAsync(connection),
        });
}
