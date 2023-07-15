namespace PoundPupLegacy.DomainModel.Creators;

internal sealed class CountrySubdivisionTypeCreatorFactory(
    IDatabaseInserterFactory<CountrySubdivisionType> countrySubdivisionTypeInserterFactory
) : IEntityCreatorFactory<CountrySubdivisionType>
{
    public async Task<IEntityCreator<CountrySubdivisionType>> CreateAsync(IDbConnection connection) =>
        new InsertingEntityCreator<CountrySubdivisionType>(new() {
            await countrySubdivisionTypeInserterFactory.CreateAsync(connection)
        });
}
