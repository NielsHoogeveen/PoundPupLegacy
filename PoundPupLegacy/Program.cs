using Microsoft.AspNetCore.Authentication.Cookies;
using PoundPupLegacy.Middleware;
using Microsoft.Extensions.FileProviders;
using Npgsql;
using PoundPupLegacy.Services;
using PoundPupLegacy.Web.Services;

namespace PoundPupLegacy;

public class Program
{
    public static async Task Main(string[] args)
    {
        const string CONNECTSTRING = "Host=localhost;Username=postgres;Password=niels;Database=ppl;Include Error Detail=True";

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Forbidden/";
            
            });
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllersWithViews();
        builder.Services.AddTransient<NpgsqlConnection>((sp) => new NpgsqlConnection(CONNECTSTRING));
        builder.Services.AddTransient<FetchNodeService>();
        builder.Services.AddTransient<FetchBlogService>();
        builder.Services.AddTransient<FetchBlogsService>();
        builder.Services.AddTransient<FetchArticlesService>();
        builder.Services.AddTransient<RazorViewToStringService>();
        builder.Services.AddTransient<StringToDocumentService>();
        builder.Services.AddTransient<TeaserService>();
        builder.Services.AddTransient<AuthenticationService>();
        builder.Services.AddSingleton<SiteDataService>();

        var app = builder.Build();


        // Configure the HTTP request pipeline.
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
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider("d:\\ppl\\files"),
            RequestPath = "/files"
        });


        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCustomMiddleware();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        var res = app.Services.GetService<SiteDataService>();
        if (res != null)
        {
            await res.InitializeAsync();
        }

        app.Run();
    }
}