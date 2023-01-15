namespace PoundPupLegacy.Middleware;

public static class CustomMiddleWare
{
    public static IApplicationBuilder UseCustomMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserMenuMiddleware>();
    }
}
