namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class UnitedStatesPoliticalPartyAffliationWriter : DatabaseWriter<UnitedStatesPoliticalPartyAffliation>, IDatabaseWriter<UnitedStatesPoliticalPartyAffliation>
{

    private const string ID = "id";
    private const string UNITED_STATES_POLITICAL_PARTY_ID = "united_states_political_party_id";
    public static async Task<DatabaseWriter<UnitedStatesPoliticalPartyAffliation>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
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
        return new UnitedStatesPoliticalPartyAffliationWriter(command);

    }

    internal UnitedStatesPoliticalPartyAffliationWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(UnitedStatesPoliticalPartyAffliation createNodeAccessPrivilege)
    {
        WriteValue(createNodeAccessPrivilege.Id, ID);
        WriteNullableValue(createNodeAccessPrivilege.UnitedStatesPoliticalPartyId, UNITED_STATES_POLITICAL_PARTY_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
