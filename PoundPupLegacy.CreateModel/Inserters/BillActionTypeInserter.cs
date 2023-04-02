namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BillActionTypeInserter : DatabaseInserter<BillActionType>, IDatabaseInserter<BillActionType>
{
    private const string ID = "id";
    public static async Task<DatabaseInserter<BillActionType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "bill_action_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                }
            }
        );
        return new BillActionTypeInserter(command);

    }

    internal BillActionTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(BillActionType personOrganizationRelationType)
    {
        if (personOrganizationRelationType.Id is null)
            throw new NullReferenceException();

        WriteValue(personOrganizationRelationType.Id, ID);
        await _command.ExecuteNonQueryAsync();
    }
}
