using System.Security.Claims;

namespace PoundPupLegacy.Services;

public interface IAuthenticationService
{
    Task<ClaimsIdentity?> Login(string userName, string password);
}
