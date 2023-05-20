namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AbuseCaseTypeOfAbuserCreator(
    IDatabaseInserterFactory<AbuseCaseTypeOfAbuser> abuseCaseTypeOfAbuserInserterFactory
) : EntityCreator<AbuseCaseTypeOfAbuser>
{
    public override async Task CreateAsync(IAsyncEnumerable<AbuseCaseTypeOfAbuser> abuseCaseTypeOfAbusers, IDbConnection connection)
    {
        await using var abuseCaseTypeOfAbuserWriter = await abuseCaseTypeOfAbuserInserterFactory.CreateAsync(connection);

        await foreach (var abuseCaseTypeOfAbuser in abuseCaseTypeOfAbusers) {
            await abuseCaseTypeOfAbuserWriter.InsertAsync(abuseCaseTypeOfAbuser);
        }
    }
}
