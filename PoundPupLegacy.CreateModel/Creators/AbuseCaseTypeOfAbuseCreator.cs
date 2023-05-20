namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class AbuseCaseTypeOfAbuseCreator(
    IDatabaseInserterFactory<AbuseCaseTypeOfAbuse> abuseCaseTypeOfAbuseInserterFactory
) : EntityCreator<AbuseCaseTypeOfAbuse>
{
    public override async Task CreateAsync(IAsyncEnumerable<AbuseCaseTypeOfAbuse> abuseCaseTypeOfAbuses, IDbConnection connection)
    {

        await using var abuseCaseTypeOfAbuseWriter = await abuseCaseTypeOfAbuseInserterFactory.CreateAsync(connection);

        await foreach (var abuseCaseTypeOfAbuse in abuseCaseTypeOfAbuses) {
            await abuseCaseTypeOfAbuseWriter.InsertAsync(abuseCaseTypeOfAbuse);
        }
    }
}
