namespace PoundPupLegacy.Db;

public class CountrySubdivisionTypeCreator : IEntityCreator<CountrySubdivisionType>
{
    public static async Task CreateAsync(IAsyncEnumerable<CountrySubdivisionType> countrySubdivisionTypes, NpgsqlConnection connection)
    {

        await using var countrySubdivisionTypeWriter = await CountrySubdivisionTypeWriter.CreateAsync(connection);

        await foreach (var countrySubdivisionType in countrySubdivisionTypes) {
            await countrySubdivisionTypeWriter.WriteAsync(countrySubdivisionType);
        }

    }
}
