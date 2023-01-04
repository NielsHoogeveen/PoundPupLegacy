namespace PoundPupLegacy.Db.Writers;

internal class InterPersonalRelationTypeWriter : DatabaseWriter<InterPersonalRelationType>, IDatabaseWriter<InterPersonalRelationType>
{
    private const string ID = "id";
    private const string IS_SYMMETRIC = "is_symmetric";

    public static async Task<DatabaseWriter<InterPersonalRelationType>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "inter_personal_relation_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = IS_SYMMETRIC,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new InterPersonalRelationTypeWriter(command);
    }

    internal InterPersonalRelationTypeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(InterPersonalRelationType interPersonalRelationType)
    {
        if (interPersonalRelationType.Id is null)
            throw new NullReferenceException();
        WriteValue(interPersonalRelationType.Id, ID);
        WriteValue(interPersonalRelationType.IsSymmetric, IS_SYMMETRIC);
        await _command.ExecuteNonQueryAsync();
    }

}
