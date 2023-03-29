namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicActionInserter : DatabaseInserter<BasicAction>, IDatabaseInserter<BasicAction>
{

    private const string ID = "id";
    private const string PATH = "path";
    private const string DESCRIPTION = "description";
    public static async Task<DatabaseInserter<BasicAction>> CreateAsync(NpgsqlConnection connection)
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
                    Name = PATH,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new BasicActionInserter(command);

    }

    internal BasicActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(BasicAction actionAccessPrivilege)
    {
        if (!actionAccessPrivilege.Id.HasValue) {
            throw new NullReferenceException();
        }
        WriteValue(actionAccessPrivilege.Id!.Value, ID);
        WriteNullableValue(actionAccessPrivilege.Path, PATH);
        WriteNullableValue(actionAccessPrivilege.Description, DESCRIPTION);
        await _command.ExecuteNonQueryAsync();
    }
}
