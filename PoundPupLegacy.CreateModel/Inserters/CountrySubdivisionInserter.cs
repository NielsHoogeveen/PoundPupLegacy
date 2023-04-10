namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CountrySubdivisionTypeInserterFactory : DatabaseInserterFactory<CountrySubdivisionType>
{
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionTypeId = new() { Name = "subdivision_type_id" };


    public override async Task<IDatabaseInserter<CountrySubdivisionType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "country_subdivision_type",
            new DatabaseParameter[] {
                CountryId,
                SubdivisionTypeId
            }
        );
        return new CountrySubdivisionTypeInserter(command);

    }

}
internal sealed class CountrySubdivisionTypeInserter : DatabaseInserter<CountrySubdivisionType>
{

    internal CountrySubdivisionTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CountrySubdivisionType countrySubdivisionType)
    {
        Set(CountrySubdivisionTypeInserterFactory.CountryId, countrySubdivisionType.CountryId);
        Set(CountrySubdivisionTypeInserterFactory.SubdivisionTypeId, countrySubdivisionType.SubdivisionTypeId);
        await _command.ExecuteNonQueryAsync();
    }
}
