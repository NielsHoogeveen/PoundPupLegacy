using System.Security.Claims;

namespace PoundPupLegacy.Services.Implementation;

internal sealed class UserService : IUserService
{
    public int GetUserId(ClaimsPrincipal claimsPrincipal)
    {

        if (claimsPrincipal == null) {
            return 0;
        }
        var useIdText = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "user_id");
        if (useIdText == null) {
            return 0;

        }
        return int.Parse(useIdText.Value);
    }

}
