namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PartyPoliticalEntityRelationTypeInserterFactory : DatabaseInserterFactory<PartyPoliticalEntityRelationType>
{
    public override async Task<IDatabaseInserter<PartyPoliticalEntityRelationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "party_political_entity_relation_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PartyPoliticalEntityRelationTypeInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PartyPoliticalEntityRelationTypeInserter.HAS_CONCRETE_SUBTYPE,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new PartyPoliticalEntityRelationTypeInserter(command);

    }

}
internal sealed class PartyPoliticalEntityRelationTypeInserter : DatabaseInserter<PartyPoliticalEntityRelationType>
{
    internal const string ID = "id";
    internal const string HAS_CONCRETE_SUBTYPE = "has_concrete_subtype";

    internal PartyPoliticalEntityRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PartyPoliticalEntityRelationType politicalEntityRelationType)
    {
        if (politicalEntityRelationType.Id is null)
            throw new NullReferenceException();

        SetParameter(politicalEntityRelationType.Id, ID);
        SetParameter(politicalEntityRelationType.HasConcreteSubtype, HAS_CONCRETE_SUBTYPE);
        await _command.ExecuteNonQueryAsync();
    }
}
