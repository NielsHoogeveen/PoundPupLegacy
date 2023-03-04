using Microsoft.AspNetCore.Authentication.Cookies;
using Npgsql;
using System.Data;
using System.Security.Claims;
using System.Security.Principal;

namespace PoundPupLegacy.Services.Implementation;

internal class AuthenticationService : IAuthenticationService
{
    private NpgsqlConnection _connection;
    public AuthenticationService(NpgsqlConnection connection)
    {
        _connection = connection;
    }

    public async Task<ClaimsIdentity?> Login(string userName, string password)
    {
        try {
            await _connection.OpenAsync();
            var sql = $"""
            select 
            p.id 
            from "user" u
            join publisher p on p.id = u.id
            where LOWER(p.name) = @name and u.password = @password
            """;

            using var readCommand = _connection.CreateCommand();
            readCommand.CommandType = CommandType.Text;
            readCommand.CommandTimeout = 300;
            readCommand.CommandText = sql;
            readCommand.Parameters.Add("name", NpgsqlTypes.NpgsqlDbType.Varchar);
            readCommand.Parameters.Add("password", NpgsqlTypes.NpgsqlDbType.Varchar);
            await readCommand.PrepareAsync();
            readCommand.Parameters["name"].Value = userName.ToLower();
            readCommand.Parameters["password"].Value = CreateMD5(password);
            await using var reader = await readCommand.ExecuteReaderAsync();
            if (reader.Read()) {
                var id = reader.GetInt32(0);
                var identity = new GenericIdentity(userName);
                var claims = new List<Claim> { new Claim("user_id", $"{id}") };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                return claimsIdentity;
            }
            return null;
        }
        finally {
            await _connection.CloseAsync();
        }
    }

    private static string CreateMD5(string input)
    {
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create()) {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes).ToLower();

        }
    }

}
