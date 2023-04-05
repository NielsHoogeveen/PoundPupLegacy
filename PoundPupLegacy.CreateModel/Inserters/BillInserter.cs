namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class BillInserterFactory : DatabaseInserterFactory<Bill>
{
    public override async Task<IDatabaseInserter<Bill>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "bill",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = BillInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = BillInserter.INTRODUCTION_DATE,
                    NpgsqlDbType = NpgsqlDbType.Date
                },
            }
        );
        return new BillInserter(command);

    }

}
internal sealed class BillInserter : DatabaseInserter<Bill>
{

    internal const string ID = "id";
    internal const string INTRODUCTION_DATE = "introduction_date";

    internal BillInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Bill bill)
    {
        WriteValue(bill.Id, ID);
        WriteNullableValue(bill.IntroductionDate, INTRODUCTION_DATE);
        await _command.ExecuteNonQueryAsync();
    }
}
