namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UnitedStatesPoliticalPartyAffliationInserterFactory : DatabaseInserterFactory<UnitedStatesPoliticalPartyAffliation>
{
    public override async Task<IDatabaseInserter<UnitedStatesPoliticalPartyAffliation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "united_states_political_party_affiliation",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = UnitedStatesPoliticalPartyAffliationInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = UnitedStatesPoliticalPartyAffliationInserter.UNITED_STATES_POLITICAL_PARTY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new UnitedStatesPoliticalPartyAffliationInserter(command);
    }
}
internal sealed class UnitedStatesPoliticalPartyAffliationInserter : DatabaseInserter<UnitedStatesPoliticalPartyAffliation>
{

    internal const string ID = "id";
    internal const string UNITED_STATES_POLITICAL_PARTY_ID = "united_states_political_party_id";

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
