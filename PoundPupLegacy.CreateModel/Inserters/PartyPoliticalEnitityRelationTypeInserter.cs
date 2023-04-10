namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PartyPoliticalEntityRelationTypeInserterFactory : DatabaseInserterFactory<PartyPoliticalEntityRelationType>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override async Task<IDatabaseInserter<PartyPoliticalEntityRelationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "party_political_entity_relation_type",
            new DatabaseParameter[] {
                Id,
                HasConcreteSubtype
            }
        );
        return new PartyPoliticalEntityRelationTypeInserter(command);

    }

}
internal sealed class PartyPoliticalEntityRelationTypeInserter : DatabaseInserter<PartyPoliticalEntityRelationType>
{
    internal PartyPoliticalEntityRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PartyPoliticalEntityRelationType politicalEntityRelationType)
    {
        if (politicalEntityRelationType.Id is null)
            throw new NullReferenceException();

        Set(PartyPoliticalEntityRelationTypeInserterFactory.Id, politicalEntityRelationType.Id.Value);
        Set(PartyPoliticalEntityRelationTypeInserterFactory.HasConcreteSubtype, politicalEntityRelationType.HasConcreteSubtype);
        await _command.ExecuteNonQueryAsync();
    }
}
