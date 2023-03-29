namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class InterPersonalRelationTypeInserter : DatabaseInserter<InterPersonalRelationType>, IDatabaseInserter<InterPersonalRelationType>
{
    private const string ID = "id";
    private const string IS_SYMMETRIC = "is_symmetric";

    public static async Task<DatabaseInserter<InterPersonalRelationType>> CreateAsync(NpgsqlConnection connection)
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
        return new InterPersonalRelationTypeInserter(command);
    }

    internal InterPersonalRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterPersonalRelationType interPersonalRelationType)
    {
        if (interPersonalRelationType.Id is null)
            throw new NullReferenceException();
        WriteValue(interPersonalRelationType.Id, ID);
        WriteValue(interPersonalRelationType.IsSymmetric, IS_SYMMETRIC);
        await _command.ExecuteNonQueryAsync();
    }

}
