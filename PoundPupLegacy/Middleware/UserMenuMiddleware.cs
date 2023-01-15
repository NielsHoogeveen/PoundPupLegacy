using PoundPupLegacy.Services;
using PoundPupLegacy.ViewModel;

namespace PoundPupLegacy.Middleware;

public class UserMenuMiddleware
{
    private readonly RequestDelegate _next;

    public UserMenuMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, UserService userService, RazorViewToStringService razorViewToStringService)
    {
        if (httpContext.User != null)
        {
            var userMenu = await userService.GetMenuItemsForUserId(httpContext.User);

            var userMenuHtml = await razorViewToStringService.GetFromView("/Views/Shared/_UserMenu.cshtml", userMenu, httpContext);

            httpContext.Items.Add("UserMenu", userMenuHtml);
            await _next(httpContext);
        }
    }
}
