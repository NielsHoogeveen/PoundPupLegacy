using System.IO;

namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicActionInserterFactory : DatabaseInserterFactory<BasicAction>
{
    public override async Task<IDatabaseInserter<BasicAction>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "basic_action",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = BasicActionInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = BasicActionInserter.PATH,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = BasicActionInserter.DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new BasicActionInserter(command);

    }

}
internal sealed class BasicActionInserter : DatabaseInserter<BasicAction>
{

    internal const string ID = "id";
    internal const string PATH = "path";
    internal const string DESCRIPTION = "description";

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
