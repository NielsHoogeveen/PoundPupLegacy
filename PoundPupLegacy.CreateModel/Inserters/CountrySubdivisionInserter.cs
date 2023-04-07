namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CountrySubdivisionTypeInserterFactory : DatabaseInserterFactory<CountrySubdivisionType>
{
    public override async Task<IDatabaseInserter<CountrySubdivisionType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "country_subdivision_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CountrySubdivisionTypeInserter.COUNTRY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CountrySubdivisionTypeInserter.SUBDIVISION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CountrySubdivisionTypeInserter(command);

    }

}
internal sealed class CountrySubdivisionTypeInserter : DatabaseInserter<CountrySubdivisionType>
{

    internal const string COUNTRY_ID = "country_id";
    internal const string SUBDIVISION_TYPE_ID = "subdivision_type_id";

    internal CountrySubdivisionTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CountrySubdivisionType countrySubdivisionType)
    {
        SetParameter(countrySubdivisionType.CountryId, COUNTRY_ID);
        SetParameter(countrySubdivisionType.SubdivisionTypeId, SUBDIVISION_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
