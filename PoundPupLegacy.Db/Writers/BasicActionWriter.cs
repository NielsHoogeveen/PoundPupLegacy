namespace PoundPupLegacy.Db.Writers;

internal sealed class BasicActionWriter : DatabaseWriter<BasicAction>, IDatabaseWriter<BasicAction>
{

    private const string ID = "id";
    private const string ACTION = "action";
    private const string DESCRIPTION = "description";
    public static async Task<DatabaseWriter<BasicAction>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "basic_action",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ACTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new BasicActionWriter(command);

    }

    internal BasicActionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(BasicAction actionAccessPrivilege)
    {
        if(actionAccessPrivilege.Id.HasValue)
        {
            throw new NullReferenceException();
        }
        WriteValue(actionAccessPrivilege.Id!.Value, ID);
        WriteNullableValue(actionAccessPrivilege.Action, ACTION);
        WriteNullableValue(actionAccessPrivilege.Description, DESCRIPTION);
        await _command.ExecuteNonQueryAsync();
    }
}
