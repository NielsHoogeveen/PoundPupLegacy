namespace PoundPupLegacy.CreateModel.Creators;

internal sealed class CountrySubdivisionTypeCreatorFactory(
    IDatabaseInserterFactory<CountrySubdivisionType> countrySubdivisionTypeInserterFactory
) : IInsertingEntityCreatorFactory<CountrySubdivisionType>
{
    public async Task<InsertingEntityCreator<CountrySubdivisionType>> CreateAsync(IDbConnection connection) =>
        new (new () {
            await countrySubdivisionTypeInserterFactory.CreateAsync(connection)
        });
}
