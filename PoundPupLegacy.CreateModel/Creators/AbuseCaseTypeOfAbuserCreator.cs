namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AbuseCaseTypeOfAbuserCreator : EntityCreator<AbuseCaseTypeOfAbuser>
{
    private readonly IDatabaseInserterFactory<AbuseCaseTypeOfAbuser> _abuseCaseTypeOfAbuserInserterFactory;
    public AbuseCaseTypeOfAbuserCreator(
        IDatabaseInserterFactory<AbuseCaseTypeOfAbuser> abuseCaseTypeOfAbuserInserterFactory
    )
    {
        _abuseCaseTypeOfAbuserInserterFactory = abuseCaseTypeOfAbuserInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<AbuseCaseTypeOfAbuser> abuseCaseTypeOfAbusers, IDbConnection connection)
    {

        await using var abuseCaseTypeOfAbuserWriter = await _abuseCaseTypeOfAbuserInserterFactory.CreateAsync(connection);

        await foreach (var abuseCaseTypeOfAbuser in abuseCaseTypeOfAbusers) {
            await abuseCaseTypeOfAbuserWriter.InsertAsync(abuseCaseTypeOfAbuser);
        }

    }
}
