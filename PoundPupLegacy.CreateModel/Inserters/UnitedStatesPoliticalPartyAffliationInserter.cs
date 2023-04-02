namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class UnitedStatesPoliticalPartyAffliationInserter : DatabaseInserter<UnitedStatesPoliticalPartyAffliation>, IDatabaseInserter<UnitedStatesPoliticalPartyAffliation>
{

    private const string ID = "id";
    private const string UNITED_STATES_POLITICAL_PARTY_ID = "united_states_political_party_id";
    public static async Task<DatabaseInserter<UnitedStatesPoliticalPartyAffliation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "united_states_political_party_affiliation",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = UNITED_STATES_POLITICAL_PARTY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new UnitedStatesPoliticalPartyAffliationInserter(command);

    }

    internal UnitedStatesPoliticalPartyAffliationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(UnitedStatesPoliticalPartyAffliation createNodeAccessPrivilege)
    {
        WriteValue(createNodeAccessPrivilege.Id, ID);
        WriteNullableValue(createNodeAccessPrivilege.UnitedStatesPoliticalPartyId, UNITED_STATES_POLITICAL_PARTY_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
