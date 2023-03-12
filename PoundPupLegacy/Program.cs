using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using Npgsql;
using PoundPupLegacy.Middleware;
using PoundPupLegacy.Services;
using PoundPupLegacy.Services.Implementation;
using Quartz;

namespace PoundPupLegacy;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Forbidden/";

            });
        //builder.Services.AddResponseCompression(options =>
        //{
        //    options.EnableForHttps = true;
        //});

        builder.Services.AddLogging(loggingBuilder => {
            loggingBuilder.AddApplicationInsights(
                        configureTelemetryConfiguration: (config) => config.ConnectionString = "InstrumentationKey=61d8fcaa-1c19-44ec-b880-4fb84d08ee5a;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/",
                        configureApplicationInsightsLoggerOptions: (options) => { }
                    );
            //loggingBuilder.AddConfiguration(builder.Configuration.GetSection("Logging"));
            //loggingBuilder.AddConsole();
            //loggingBuilder.AddDebug();
        });
        builder.Services.AddSignalR(e => {
            e.MaximumReceiveMessageSize = 102400000;
        });
        builder.Services.AddSingleton<ISiteDataService, SiteDataService>();
        builder.Services.AddSingleton<INodeCacheService, NodeCacheService>();
        builder.Services.AddControllersWithViews();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddTransient((sp) => {
            var configuration = sp.GetService<IConfiguration>()!;
            var connectString = configuration["ConnectString"]!;
            return new NpgsqlConnection(connectString);
        });
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
        builder.Services.AddTransient<ISubgroupService, SubgroupService>();
        builder.Services.AddTransient<ITopicService, TopicService>();
        builder.Services.AddTransient<IPersonService, PersonService>();
        builder.Services.AddTransient<IDocumentableDocumentsSearchService, DocumentableDocumentsSearchService>();
        builder.Services.AddTransient<IAttachmentService, AttachmentService>();
        builder.Services.AddTransient<IUserService, UserService>();

        builder.Services.AddQuartz(q => {
            // base Quartz scheduler, job and trigger configuration
        });

        // ASP.NET Core hosting
        builder.Services.AddQuartzServer(options => {
            // when shutting down we want jobs to complete gracefully
            options.WaitForJobsToComplete = true;
        });

        builder.Services.AddApplicationInsightsTelemetry();

        var app = builder.Build();


        //if (!app.Environment.IsDevelopment())
        //{
        //    app.UseResponseCompression();
        //}

        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

        }
        var cookiePolicyOptions = new CookiePolicyOptions {
            MinimumSameSitePolicy = SameSiteMode.Strict,
        };



        app.UseCookiePolicy(cookiePolicyOptions);

        app.Use(async (context, next) => {
            await next();
            if (context.Response.StatusCode == 404) {
                context.Request.Path = "/NotFound";
                await next();
            }
        });
        app.UseHttpsRedirection();
        app.UseStaticFiles();

        var configuration = app.Services.GetService<IConfiguration>()!;
        var filesPath = configuration["FilesLocation"]!;
        app.UseStaticFiles(new StaticFileOptions {

            FileProvider = new PhysicalFileProvider(filesPath),
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
        if (res != null) {
            await res.InitializeAsync();
        }

        app.Run();
    }
}
