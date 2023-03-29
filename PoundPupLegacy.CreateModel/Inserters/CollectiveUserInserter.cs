namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CollectiveUserInserter : DatabaseInserter<CollectiveUser>, IDatabaseInserter<CollectiveUser>
{
    private const string COLLECTIVE_ID = "collective_id";
    private const string USER_ID = "user_id";
    public static async Task<DatabaseInserter<CollectiveUser>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "collective_user",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = COLLECTIVE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = USER_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CollectiveUserInserter(command);

    }

    internal CollectiveUserInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CollectiveUser collectiveUser)
    {
        if (collectiveUser.CollectiveId is null || collectiveUser.UserId is null)
            throw new NullReferenceException();
        WriteValue(collectiveUser.CollectiveId, COLLECTIVE_ID);
        WriteValue(collectiveUser.UserId, USER_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
