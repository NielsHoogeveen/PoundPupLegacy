using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.ViewModel.Readers;

public class PasswordValidationReader : DatabaseReader, IDatabaseReader<PasswordValidationReader>
{
    private PasswordValidationReader(NpgsqlCommand command) : base(command)
    {
    }

    public async Task<int?> ReadAsync(string userName, string password)
    {
        _command.Parameters["name"].Value = userName.ToLower();
        _command.Parameters["password"].Value = password;
        await using var reader = await _command.ExecuteReaderAsync();
        if (reader.Read()) {
            return reader.GetInt32(0);
        }
        else {
            return null;
        }
    }

    public static async Task<PasswordValidationReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
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
