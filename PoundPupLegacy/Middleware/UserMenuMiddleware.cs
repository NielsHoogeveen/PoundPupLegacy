using Microsoft.AspNetCore.Components.Authorization;
using PoundPupLegacy.Services;

namespace PoundPupLegacy.Middleware;

public class UserMenuMiddleware
{
    private readonly RequestDelegate _next;

    public UserMenuMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext httpContext, 
        IUserService userService,
        ISiteDataService siteDataService, 
        IRazorViewToStringService razorViewToStringService,
        AuthenticationStateProvider authenticationStateProvider)
    {
        var userId = userService.GetUserId(httpContext.User);
        var tenantId = siteDataService.GetTenantId(httpContext.Request);
        var userMenu = siteDataService.GetMenuItemsForUser(userId, tenantId);

        var userMenuHtml = await razorViewToStringService.GetFromView("/Views/Shared/_UserMenu.cshtml", userMenu);
        httpContext.Items.Remove("UserMenu");
        httpContext.Items.Add("UserMenu", userMenuHtml);
        await _next(httpContext);
    }
}
