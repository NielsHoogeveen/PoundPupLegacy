namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class InterCountryRelationTypeInserter : DatabaseInserter<InterCountryRelationType>, IDatabaseInserter<InterCountryRelationType>
{
    private const string ID = "id";
    private const string IS_SYMMETRIC = "is_symmetric";

    public static async Task<DatabaseInserter<InterCountryRelationType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
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
        return new InterCountryRelationTypeInserter(command);
    }

    internal InterCountryRelationTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(InterCountryRelationType interCountryRelationType)
    {
        if (interCountryRelationType.Id is null)
            throw new NullReferenceException();
        WriteValue(interCountryRelationType.Id, ID);
        WriteValue(interCountryRelationType.IsSymmetric, IS_SYMMETRIC);
        await _command.ExecuteNonQueryAsync();
    }

}
