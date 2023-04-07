namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BillActionTypeInserterFactory : DatabaseInserterFactory<BillActionType>
{
    public override async Task<IDatabaseInserter<BillActionType>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "bill_action_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = BillActionTypeInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                }
            }
        );
        return new BillActionTypeInserter(command);

    }

}
internal sealed class BillActionTypeInserter : DatabaseInserter<BillActionType>
{
    internal const string ID = "id";

    internal BillActionTypeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(BillActionType billActionType)
    {
        if (billActionType.Id is null)
            throw new NullReferenceException();

        SetParameter(billActionType.Id, ID);
        await _command.ExecuteNonQueryAsync();
    }
}
