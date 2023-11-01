using Microsoft.AspNetCore.Components.Server.Circuits;
using PoundPupLegacy.Services;
using PoundPupLegacy.Services.Implementation;

namespace PoundPupLegacy;

public class CircuitHandlerService(IHttpContextAccessor httpContextAccessor, IActiveUserService activeUserService, IUserService userService) : CircuitHandler
{
    public override async Task OnCircuitOpenedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        await base.OnCircuitOpenedAsync(circuit, cancellationToken);
        var user = httpContextAccessor.HttpContext?.User;
        if(user == null) {
            return;
        }
        var userInfo = user.Identity is not null
            ? await userService.GetUserInfo(user)
            : new UserLookupResponse.NoUser();
        if(userInfo is UserLookupResponse.ExistingUser existingUser) {
            await activeUserService.RegisterActiveUser(existingUser.user);
        }
    }
    public override async Task OnCircuitClosedAsync(Circuit circuit, CancellationToken cancellationToken)
    {
        await base.OnCircuitClosedAsync(circuit, cancellationToken);
        var user = httpContextAccessor.HttpContext?.User;
        if (user == null) {
            return;
        }
        var userInfo = user.Identity is not null
            ? await userService.GetUserInfo(user)
            : new UserLookupResponse.NoUser();
        if (userInfo is UserLookupResponse.ExistingUser existingUser) {
            await activeUserService.UnRegisterActiveUser(existingUser.user, false);
        }
    }
}

