namespace PoundPupLegacy.Db.Writers;

internal sealed class CollectiveUserWriter : DatabaseWriter<CollectiveUser>, IDatabaseWriter<CollectiveUser>
{
    private const string COLLECTIVE_ID = "collective_id";
    private const string USER_ID = "user_id";
    public static async Task<DatabaseWriter<CollectiveUser>> CreateAsync(NpgsqlConnection connection)
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
        return new CollectiveUserWriter(command);

    }

    internal CollectiveUserWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(CollectiveUser collectiveUser)
    {
        if (collectiveUser.CollectiveId is null || collectiveUser.UserId is null)
            throw new NullReferenceException();
        WriteValue(collectiveUser.CollectiveId, COLLECTIVE_ID);
        WriteValue(collectiveUser.UserId, USER_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
