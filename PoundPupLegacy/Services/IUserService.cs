using System.Security.Claims;

namespace PoundPupLegacy.Services;

public interface IUserService
{
    int GetUserId(ClaimsPrincipal claimsPrincipal);
}
