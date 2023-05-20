namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CountrySubdivisionTypeCreator(IDatabaseInserterFactory<CountrySubdivisionType> countrySubdivisionTypeInserterFactory) : EntityCreator<CountrySubdivisionType>
{
    public override async Task CreateAsync(IAsyncEnumerable<CountrySubdivisionType> countrySubdivisionTypes, IDbConnection connection)
    {
        await using var countrySubdivisionTypeWriter = await countrySubdivisionTypeInserterFactory.CreateAsync(connection);

        await foreach (var countrySubdivisionType in countrySubdivisionTypes) {
            await countrySubdivisionTypeWriter.InsertAsync(countrySubdivisionType);
        }
    }
}
