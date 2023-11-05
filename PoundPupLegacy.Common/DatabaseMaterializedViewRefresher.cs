using Npgsql;
using System.Data;

namespace PoundPupLegacy.Common;

public class RefresherRequest: IRequest
{

}
public class DatabaseMaterializedViewRefresherFactory : DatabaseAccessorFactory
{
    public async Task<DatabaseMaterializedViewRefresher> CreateAsync(IDbConnection connection, string name)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = $"REFRESH MATERIALIZED VIEW {name}";

        foreach (var parameter in DatabaseParameters) {
            command.AddParameter(parameter);
        }
        await command.PrepareAsync();
        return new DatabaseMaterializedViewRefresher(command);

    }
}
public class DatabaseMaterializedViewRefresher : DatabaseAccessor<RefresherRequest>
{
    public DatabaseMaterializedViewRefresher(NpgsqlCommand command) : base(command)
    {
    }

    public async Task Execute()
    {
        await _command.ExecuteNonQueryAsync();
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(RefresherRequest request)
    {
        return Array.Empty<ParameterValue>();
    }
}
