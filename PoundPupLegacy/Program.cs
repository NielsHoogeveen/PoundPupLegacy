using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using Npgsql;
using PoundPupLegacy.Middleware;
using PoundPupLegacy.Services;
using PoundPupLegacy.Services.Implementation;

namespace PoundPupLegacy;

public class Program
{
    public static async Task Main(string[] args)
    {
        const string CONNECTSTRING = "Host=localhost;Username=postgres;Password=niels;Database=ppl;Include Error Detail=True";

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Forbidden/";

            });
        //builder.Services.AddResponseCompression(options =>
        //{
        //    options.EnableForHttps = true;
        //});
        builder.Services.AddSignalR(e => {
            e.MaximumReceiveMessageSize = 102400000;
        });
        builder.Services.AddSingleton<ISiteDataService, SiteDataService>();
        builder.Services.AddSingleton<INodeCacheService, NodeCacheService>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllersWithViews();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddTransient((sp) => new NpgsqlConnection(CONNECTSTRING));
        builder.Services.AddTransient<IFetchNodeService, FetchNodeService>();
        builder.Services.AddTransient<IFetchBlogService, FetchBlogService>();
        builder.Services.AddTransient<IFetchBlogsService, FetchBlogsService>();
        builder.Services.AddTransient<IFetchArticlesService, FetchArticlesService>();
        builder.Services.AddTransient<IFetchPollsService, FetchPollsService>();
        builder.Services.AddTransient<IFetchOrganizationsService, FetchOrganizationsService>();
        builder.Services.AddTransient<IFetchCasesService, FetchCasesService>();
        builder.Services.AddTransient<IFetchCountriesService, FetchCountriesService>();
        builder.Services.AddTransient<IFetchSearchService, FetchSearchService>();
        builder.Services.AddTransient<IRazorViewToStringService, RazorViewToStringService>();
        builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
        builder.Services.AddTransient<IEditorService, EditorService>();
        builder.Services.AddTransient<ITextService, TextService>();
        builder.Services.AddTransient<ITopicSearchService, TopicSearchService>();
        builder.Services.AddTransient<ICongressionalDataService, CongressionalDataService>();
        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        //if (!app.Environment.IsDevelopment())
        //{
        //    app.UseResponseCompression();
        //}

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

        }
        var cookiePolicyOptions = new CookiePolicyOptions
        {
            MinimumSameSitePolicy = SameSiteMode.Strict,
        };



        app.UseCookiePolicy(cookiePolicyOptions);

        app.Use(async (context, next) =>
        {
            await next();
            if (context.Response.StatusCode == 404)
            {
                context.Request.Path = "/NotFound";
                await next();
            }
        });
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider("\\\\wsl.localhost\\Ubuntu\\home\\niels\\files\\files"),
            RequestPath = "/files"
        });

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();


        app.UseCustomMiddleware();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
           name: "all-else",
           pattern: "{*url}",
           defaults: new { controller = "Home", action = "AllElse" });

        app.MapBlazorHub();

        var res = app.Services.GetService<ISiteDataService>();
        if (res != null)
        {
            await res.InitializeAsync();
        }

        app.Run();
    }
}
