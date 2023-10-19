using System.Security.Claims;

namespace PoundPupLegacy.Services;

public interface IUserService
{
    Task<int> GetUserId(ClaimsPrincipal claimsPrincipal);
}
