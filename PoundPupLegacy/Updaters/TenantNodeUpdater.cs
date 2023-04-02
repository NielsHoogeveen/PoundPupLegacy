using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.Updaters;

internal sealed class TenantNodeUpdaterFactory : IDatabaseUpdaterFactory<TenantNodeUpdater>
{
    public async Task<TenantNodeUpdater> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        var sql = $"""
                update tenant_node 
                set 
                url_path = @url_path, 
                subgroup_id = @subgroup_id, 
                publication_status_id = @publication_status_id
                where id = @id
                """;
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        command.Parameters.Add("id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("url_path", NpgsqlTypes.NpgsqlDbType.Varchar);
        command.Parameters.Add("subgroup_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("publication_status_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new TenantNodeUpdater(command);
    }
}

internal sealed class TenantNodeUpdater : DatabaseUpdater<TenantNodeUpdater.Request>
{
    public TenantNodeUpdater(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task UpdateAsync(Request request)
    {
        _command.Parameters["id"].Value = request.Id;
        if (string.IsNullOrEmpty(request.UrlPath)) {
            _command.Parameters["url_path"].Value = DBNull.Value;
        }
        else {
            _command.Parameters["url_path"].Value = request.UrlPath;
        }
        if (request.SubgroupId.HasValue) {
            _command.Parameters["subgroup_id"].Value = request.SubgroupId;
        }
        else {
            _command.Parameters["subgroup_id"].Value = DBNull.Value;
        }
        _command.Parameters["publication_status_id"].Value = request.PublicationStatusId;
        await _command.ExecuteNonQueryAsync();
    }

    public record Request
    {
        public required int Id { get; init; }
        public required string? UrlPath { get; init; }
        public required int? SubgroupId { get; init; }
        public required int? PublicationStatusId { get; init; }
    }
}
