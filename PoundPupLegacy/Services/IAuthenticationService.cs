using PoundPupLegacy.Common;
using System.Security.Claims;

namespace PoundPupLegacy.Services;

public interface IAuthenticationService
{
    [RequireNamedArgs]
    Task<ClaimsIdentity?> Login(string userName, string password);
}
