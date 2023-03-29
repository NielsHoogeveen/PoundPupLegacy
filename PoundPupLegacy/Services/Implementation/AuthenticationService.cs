using Microsoft.AspNetCore.Authentication.Cookies;
using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Readers;
using System.Data;
using System.Security.Claims;
using System.Security.Principal;

namespace PoundPupLegacy.Services.Implementation;

internal class AuthenticationService : IAuthenticationService
{
    private NpgsqlConnection _connection;
    private readonly IDatabaseReaderFactory<PasswordValidationReader> _passwordValidationReaderFactory;
    public AuthenticationService(
        NpgsqlConnection connection,
        IDatabaseReaderFactory<PasswordValidationReader> passwordValidationReaderFactory)
    {
        _connection = connection;
        _passwordValidationReaderFactory = passwordValidationReaderFactory;

    }

    public async Task<ClaimsIdentity?> Login(string userName, string password)
    {
        string CreateMD5(string input)
        {
            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes).ToLower();
        }
        try {
            await _connection.OpenAsync();

            await using var reader = await _passwordValidationReaderFactory.CreateAsync(_connection);
            var userId = await reader.ReadAsync(new PasswordValidationReader.PasswordValidationRequest {
                Password = CreateMD5(password),
                UserName = userName.ToLower()
            });
            if (userId is not null) {
                var id = userId;
                var identity = new GenericIdentity(userName);
                var claims = new List<Claim> { new Claim("user_id", $"{id}") };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                return claimsIdentity;
            }
            return null;
        }
        finally {
            if (_connection.State == ConnectionState.Open) {
                await _connection.CloseAsync();
            }
        }
    }
}
