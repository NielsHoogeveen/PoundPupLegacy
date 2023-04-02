namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicActionInserter : DatabaseInserter<BasicAction>, IDatabaseInserter<BasicAction>
{

    private const string ID = "id";
    private const string PATH = "path";
    private const string DESCRIPTION = "description";
    public static async Task<DatabaseInserter<BasicAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
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

    public override async Task InsertAsync(BasicAction actionAccessPrivilege)
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
