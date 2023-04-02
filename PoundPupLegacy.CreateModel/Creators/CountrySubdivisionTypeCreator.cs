namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CountrySubdivisionTypeCreator : IEntityCreator<CountrySubdivisionType>
{
    public async Task CreateAsync(IAsyncEnumerable<CountrySubdivisionType> countrySubdivisionTypes, IDbConnection connection)
    {

        await using var countrySubdivisionTypeWriter = await CountrySubdivisionTypeWriter.CreateAsync(connection);

        await foreach (var countrySubdivisionType in countrySubdivisionTypes) {
            await countrySubdivisionTypeWriter.InsertAsync(countrySubdivisionType);
        }

    }
}
