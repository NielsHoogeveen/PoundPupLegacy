namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AbuseCaseTypeOfAbuseCreator : EntityCreator<AbuseCaseTypeOfAbuse>
{
    private readonly IDatabaseInserterFactory<AbuseCaseTypeOfAbuse> _abuseCaseTypeOfAbuseInserterFactory;
    public AbuseCaseTypeOfAbuseCreator(
        IDatabaseInserterFactory<AbuseCaseTypeOfAbuse> abuseCaseTypeOfAbuseInserterFactory
    )
    {
        _abuseCaseTypeOfAbuseInserterFactory = abuseCaseTypeOfAbuseInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<AbuseCaseTypeOfAbuse> abuseCaseTypeOfAbuses, IDbConnection connection)
    {

        await using var abuseCaseTypeOfAbuseWriter = await _abuseCaseTypeOfAbuseInserterFactory.CreateAsync(connection);

        await foreach (var abuseCaseTypeOfAbuse in abuseCaseTypeOfAbuses) {
            await abuseCaseTypeOfAbuseWriter.InsertAsync(abuseCaseTypeOfAbuse);
        }

    }
}
