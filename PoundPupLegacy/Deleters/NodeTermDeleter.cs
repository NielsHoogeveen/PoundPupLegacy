﻿using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel;
using System.Data;

namespace PoundPupLegacy.Deleters;
internal sealed class NodeTermDeleterFactory : IDatabaseDeleterFactory<NodeTermDeleter>
{
    public async Task<NodeTermDeleter> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = postgresConnection.CreateCommand();

        var sql = $"""
                    delete from node_term
                    where node_id = @node_id and term_id = @term_id;
                    """;
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = sql;
        command.Parameters.Add("node_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("term_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new NodeTermDeleter(command);
    }
}
internal sealed class NodeTermDeleter : DatabaseDeleter<NodeTermDeleter.Request>
{
    public record Request
    {
        public required int NodeId { get; init; }
        public required int TermId { get; init; }
    }
    public NodeTermDeleter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task DeleteAsync(Request request)
    {
        _command.Parameters["node_id"].Value = request.NodeId;
        _command.Parameters["term_id"].Value = request.TermId;
        await _command.ExecuteNonQueryAsync();
    }
}
