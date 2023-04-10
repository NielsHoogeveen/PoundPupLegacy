namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BoundCountryInserterFactory : DatabaseInserterFactory<BoundCountry>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter BindingCountryId = new() { Name = "binding_country_id" };

    public override async Task<IDatabaseInserter<BoundCountry>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "bound_country",
            new DatabaseParameter[] {
                Id,
                BindingCountryId
            }
        );
        return new BoundCountryInserter(command);
    }

}
internal sealed class BoundCountryInserter : DatabaseInserter<BoundCountry>
{
    internal BoundCountryInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(BoundCountry country)
    {
        if (country.Id is null)
            throw new NullReferenceException();
        Set(BoundCountryInserterFactory.Id, country.Id.Value);
        Set(BoundCountryInserterFactory.BindingCountryId, country.BindingCountryId);
        await _command.ExecuteNonQueryAsync();
    }
}
