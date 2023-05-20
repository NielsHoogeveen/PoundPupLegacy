using Microsoft.AspNetCore.Authentication.Cookies;
using PoundPupLegacy.Common;
using PoundPupLegacy.Readers;
using System.Data;
using System.Security.Claims;
using System.Security.Principal;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class AuthenticationService(
        IDbConnection connection,
        ILogger<AuthenticationService> logger,
        ISingleItemDatabaseReaderFactory<PasswordValidationReaderRequest, PasswordValidationReaderResponse> passwordValidationReaderFactory
    ) : DatabaseService(connection, logger), IAuthenticationService
{
    public async Task<ClaimsIdentity?> Login(string userName, string password)
    {
        string CreateMD5(string input)
        {
            using System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            return Convert.ToHexString(hashBytes).ToLower();
        }
        return await WithConnection(async (connection) => {

            await using var reader = await passwordValidationReaderFactory.CreateAsync(connection);
            var response = await reader.ReadAsync(new PasswordValidationReaderRequest {
                Password = CreateMD5(password),
                UserName = userName.ToLower()
            });
            if (response is not null) {
                var id = response.UserId;
                var identity = new GenericIdentity(userName);
                var claims = new List<Claim> { new Claim("user_id", $"{id}") };
                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                return claimsIdentity;
            }
            return null;
        });
    }
}
