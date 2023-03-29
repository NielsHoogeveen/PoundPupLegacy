namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class BillActionTypeWriter : DatabaseWriter<BillActionType>, IDatabaseWriter<BillActionType>
{
    private const string ID = "id";
    public static async Task<DatabaseWriter<BillActionType>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "bill_action_type",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                }
            }
        );
        return new BillActionTypeWriter(command);

    }

    internal BillActionTypeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(BillActionType personOrganizationRelationType)
    {
        if (personOrganizationRelationType.Id is null)
            throw new NullReferenceException();

        WriteValue(personOrganizationRelationType.Id, ID);
        await _command.ExecuteNonQueryAsync();
    }
}
