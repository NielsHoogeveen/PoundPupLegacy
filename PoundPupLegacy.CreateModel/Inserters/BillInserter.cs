namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BillInserter : DatabaseInserter<Bill>, IDatabaseInserter<Bill>
{

    private const string ID = "id";
    private const string INTRODUCTION_DATE = "introduction_date";
    public static async Task<DatabaseInserter<Bill>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "bill",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = INTRODUCTION_DATE,
                    NpgsqlDbType = NpgsqlDbType.Date
                },
            }
        );
        return new BillInserter(command);

    }

    internal BillInserter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Bill bill)
    {
        WriteValue(bill.Id, ID);
        WriteNullableValue(bill.IntroductionDate, INTRODUCTION_DATE);
        await _command.ExecuteNonQueryAsync();
    }
}
