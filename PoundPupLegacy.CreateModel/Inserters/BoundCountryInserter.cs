namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BoundCountryInserterFactory : DatabaseInserterFactory<BoundCountry>
{
    public override async Task<IDatabaseInserter<BoundCountry>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "bound_country",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = BoundCountryInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = BoundCountryInserter.BINDING_COUNTRY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new BoundCountryInserter(command);
    }

}
internal sealed class BoundCountryInserter : DatabaseInserter<BoundCountry>
{
    internal const string ID = "id";
    internal const string BINDING_COUNTRY_ID = "binding_country_id";
    internal BoundCountryInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(BoundCountry country)
    {
        if (country.Id is null)
            throw new NullReferenceException();
        SetParameter(country.Id, ID);
        SetParameter(country.BindingCountryId, BINDING_COUNTRY_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
