namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BoundCountryInserter : DatabaseInserter<BoundCountry>, IDatabaseInserter<BoundCountry>
{
    private const string ID = "id";
    private const string BINDING_COUNTRY_ID = "binding_country_id";
    public static async Task<DatabaseInserter<BoundCountry>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "bound_country",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = BINDING_COUNTRY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new BoundCountryInserter(command);
    }
    private BoundCountryInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(BoundCountry country)
    {
        if (country.Id is null)
            throw new NullReferenceException();
        WriteValue(country.Id, ID);
        WriteValue(country.BindingCountryId, BINDING_COUNTRY_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
