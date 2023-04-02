using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.Deleters;
internal sealed class TenantNodeDeleterFactory : IDatabaseDeleterFactory<TenantNodeDeleter>
{
    public async Task<TenantNodeDeleter> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = postgresConnection.CreateCommand();

        var sql = $"""
                delete from tenant_node
                where id = @id;
                """;
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        command.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new TenantNodeDeleter(command);
    }
}
internal sealed class TenantNodeDeleter : DatabaseDeleter<int>
{
    public TenantNodeDeleter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task DeleteAsync(int id)
    {
        _command.Parameters["id"].Value = id;
         await _command.ExecuteNonQueryAsync();
    }
}
