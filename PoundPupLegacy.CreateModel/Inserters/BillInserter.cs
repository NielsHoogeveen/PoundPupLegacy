namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class BillInserterFactory : DatabaseInserterFactory<Bill>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableDateTimeDatabaseParameter IntroductionDate = new() { Name = "introduction_date" };

    public override async Task<IDatabaseInserter<Bill>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "bill",
            new DatabaseParameter[] {
                Id,
                IntroductionDate
            }
        );
        return new BillInserter(command);

    }

}
internal sealed class BillInserter : DatabaseInserter<Bill>
{


    internal BillInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Bill bill)
    {
        if (bill.Id is null)
            throw new ArgumentNullException(nameof(bill.Id));
        Set(BillInserterFactory.Id, bill.Id.Value);
        Set(BillInserterFactory.IntroductionDate, bill.IntroductionDate);
        await _command.ExecuteNonQueryAsync();
    }
}
