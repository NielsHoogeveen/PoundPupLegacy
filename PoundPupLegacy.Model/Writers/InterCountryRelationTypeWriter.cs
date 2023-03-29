namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class InterCountryRelationTypeWriter : DatabaseWriter<InterCountryRelationType>, IDatabaseWriter<InterCountryRelationType>
{
    private const string ID = "id";
    private const string IS_SYMMETRIC = "is_symmetric";

    public static async Task<DatabaseWriter<InterCountryRelationType>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "inter_country_relation_type",
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
        return new InterCountryRelationTypeWriter(command);
    }

    internal InterCountryRelationTypeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(InterCountryRelationType interCountryRelationType)
    {
        if (interCountryRelationType.Id is null)
            throw new NullReferenceException();
        WriteValue(interCountryRelationType.Id, ID);
        WriteValue(interCountryRelationType.IsSymmetric, IS_SYMMETRIC);
        await _command.ExecuteNonQueryAsync();
    }

}
