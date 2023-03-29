namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BillActionTypeInserter : DatabaseInserter<BillActionType>, IDatabaseInserter<BillActionType>
{
    private const string ID = "id";
    public static async Task<DatabaseInserter<BillActionType>> CreateAsync(NpgsqlConnection connection)
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
