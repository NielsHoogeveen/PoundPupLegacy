namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class InterOrganizationalRelationTypeInserter : DatabaseInserter<InterOrganizationalRelationType>, IDatabaseInserter<InterOrganizationalRelationType>
{
    private const string ID = "id";
    private const string IS_SYMMETRIC = "is_symmetric";

    public static async Task<DatabaseInserter<InterOrganizationalRelationType>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "inter_organizational_relation_type",
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
        return new InterOrganizationalRelationTypeInserter(command);
    }

    internal InterOrganizationalRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(InterOrganizationalRelationType interOrganizationalRelationType)
    {
        if (interOrganizationalRelationType.Id is null)
            throw new NullReferenceException();
        WriteValue(interOrganizationalRelationType.Id, ID);
        WriteValue(interOrganizationalRelationType.IsSymmetric, IS_SYMMETRIC);
        await _command.ExecuteNonQueryAsync();
    }

}
