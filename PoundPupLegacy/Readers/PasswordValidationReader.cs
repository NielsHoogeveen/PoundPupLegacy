﻿using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.Readers;
internal sealed class PasswordValidationReaderFactory : IDatabaseReaderFactory<PasswordValidationReader>
{
    public async Task<PasswordValidationReader> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        command.Parameters.Add("name", NpgsqlTypes.NpgsqlDbType.Varchar);
        command.Parameters.Add("password", NpgsqlTypes.NpgsqlDbType.Varchar);
        await command.PrepareAsync();
        return new PasswordValidationReader(command);
    }

    const string SQL = """
        select 
        p.id 
        from "user" u
        join publisher p on p.id = u.id
        where LOWER(p.name) = @name and u.password = @password
        """;


}
internal sealed class PasswordValidationReader : SingleItemDatabaseReader<PasswordValidationReader.Request, int?>
{
    public record Request
    {
        public required string UserName { get; init; }
        public required string Password { get; init; }
    }
    internal PasswordValidationReader(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task<int?> ReadAsync(Request request)
    {
        _command.Parameters["name"].Value = request.UserName;
        _command.Parameters["password"].Value = request.Password;
        await using var reader = await _command.ExecuteReaderAsync();
        if (reader.Read()) {
            return reader.GetInt32(0);
        }
        else {
            return null;
        }
    }

}
