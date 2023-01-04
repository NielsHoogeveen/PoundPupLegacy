namespace PoundPupLegacy.Db.Writers;

internal class PoliticalEntityRelationTypeWriter : DatabaseWriter<PoliticalEntityRelationType>, IDatabaseWriter<PoliticalEntityRelationType>
{
    private const string ID = "id";
    private const string HAS_CONCRETE_SUBTYPE = "has_concrete_subtype";
    public static async Task<DatabaseWriter<PoliticalEntityRelationType>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "political_entity_relation_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = HAS_CONCRETE_SUBTYPE,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new PoliticalEntityRelationTypeWriter(command);

    }

    internal PoliticalEntityRelationTypeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(PoliticalEntityRelationType politicalEntityRelationType)
    {
        if (politicalEntityRelationType.Id is null)
            throw new NullReferenceException();

        WriteValue(politicalEntityRelationType.Id, ID);
        WriteValue(politicalEntityRelationType.HasConcreteSubtype, HAS_CONCRETE_SUBTYPE);
        await _command.ExecuteNonQueryAsync();
    }
}
