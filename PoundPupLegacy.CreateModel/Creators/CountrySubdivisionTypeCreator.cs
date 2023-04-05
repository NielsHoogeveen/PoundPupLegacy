namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CountrySubdivisionTypeCreator : EntityCreator<CountrySubdivisionType>
{
    private readonly IDatabaseInserterFactory<CountrySubdivisionType> _countrySubdivisionTypeInserterFactory;
    public CountrySubdivisionTypeCreator(IDatabaseInserterFactory<CountrySubdivisionType> countrySubdivisionTypeInserterFactory)
    {
        _countrySubdivisionTypeInserterFactory = countrySubdivisionTypeInserterFactory;
    }
    public override async Task CreateAsync(IAsyncEnumerable<CountrySubdivisionType> countrySubdivisionTypes, IDbConnection connection)
    {

        await using var countrySubdivisionTypeWriter = await _countrySubdivisionTypeInserterFactory.CreateAsync(connection);

        await foreach (var countrySubdivisionType in countrySubdivisionTypes) {
            await countrySubdivisionTypeWriter.InsertAsync(countrySubdivisionType);
        }

    }
}
