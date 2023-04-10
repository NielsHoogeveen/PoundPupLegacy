namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UnitedStatesPoliticalPartyAffliationInserterFactory : DatabaseInserterFactory<UnitedStatesPoliticalPartyAffliation>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableIntegerDatabaseParameter UnitedStatsPoliticalPartyId = new() { Name = "united_states_political_party_id" };

    public override async Task<IDatabaseInserter<UnitedStatesPoliticalPartyAffliation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "united_states_political_party_affiliation",
            new DatabaseParameter[] {
                Id,
                UnitedStatsPoliticalPartyId
            }
        );
        return new UnitedStatesPoliticalPartyAffliationInserter(command);
    }
}
internal sealed class UnitedStatesPoliticalPartyAffliationInserter : DatabaseInserter<UnitedStatesPoliticalPartyAffliation>
{
    internal UnitedStatesPoliticalPartyAffliationInserter(NpgsqlCommand command) : base(command)
    {
    }
    public override async Task InsertAsync(UnitedStatesPoliticalPartyAffliation createNodeAccessPrivilege)
    {
        if (createNodeAccessPrivilege.Id is null)
            throw new NullReferenceException();
        Set(UnitedStatesPoliticalPartyAffliationInserterFactory.Id, createNodeAccessPrivilege.Id.Value);
        Set(UnitedStatesPoliticalPartyAffliationInserterFactory.UnitedStatsPoliticalPartyId, createNodeAccessPrivilege.UnitedStatesPoliticalPartyId);
        await _command.ExecuteNonQueryAsync();
    }
}
