using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Data;
using PoundPupLegacy.Services;
using Quartz;
using Quartz.AspNetCore;
using System.Text.Json.Serialization.Metadata;
using PoundPupLegacy.Areas.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Components.Server.Circuits;
using System.Diagnostics;

namespace PoundPupLegacy;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        IJsonTypeInfoResolver[] resolvers = Extensions.GetResolvers();
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOptions();
        builder.Services.AddMemoryCache();

        builder.Services.AddHttpContextAccessor();
        var connectionString = builder.Configuration.GetValue<string>("ConnectString")!;

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();


        builder.Services.AddAuthentication()
            .AddProviders(builder.Configuration.GetSection("OAuth2Providers").Get<List<OAuthProvider>>()!);
        ;
        

        builder.Services.AddLogging(loggingBuilder => {
            loggingBuilder.AddApplicationInsights(
                configureTelemetryConfiguration: (config) => config.ConnectionString = "InstrumentationKey=61d8fcaa-1c19-44ec-b880-4fb84d08ee5a;IngestionEndpoint=https://eastus-8.in.applicationinsights.azure.com/;LiveEndpoint=https://eastus.livediagnostics.monitor.azure.com/",
                configureApplicationInsightsLoggerOptions: (options) => { }
            );
        });
        builder.Services.AddScoped<NotFoundListener>();
        builder.Services.AddSignalR(e => {
            e.MaximumReceiveMessageSize = 102400000;
        });

        builder.Services.AddRazorPages()
            .AddJsonOptions(options => {
                 options
                     .JsonSerializerOptions
                     .Converters.Add(FuzzyDateJsonConverter.Default);

                 options
                     .JsonSerializerOptions
                     .TypeInfoResolver = JsonTypeInfoResolver.Combine(resolvers);

             });

        builder.Services.AddControllersWithViews()
            .AddJsonOptions(options => {
                options
                    .JsonSerializerOptions
                    .Converters.Add(FuzzyDateJsonConverter.Default);

                options
                    .JsonSerializerOptions
                    .TypeInfoResolver = JsonTypeInfoResolver.Combine(resolvers);

            });

        builder.Services.AddServerSideBlazor();
        
        builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

        builder.Services.AddSingleton((sp) => {
            var configuration = sp.GetService<IConfiguration>()!;
            var connectString = configuration["ConnectString"]!;
            var dataSource = new NpgsqlDataSourceBuilder(connectString)
                //.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>())
                .EnableDynamicJsonMappings(new System.Text.Json.JsonSerializerOptions {
                   TypeInfoResolver = JsonTypeInfoResolver.Combine(resolvers),
                    Converters = { FuzzyDateJsonConverter.Default }
                })
                .Build();
            return dataSource;
        });
        builder.Services.AddApplicationServices();
        builder.Services.AddQuartz(q => {
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();
            q.UseDefaultThreadPool(tp => {
                tp.MaxConcurrency = 1;
            });
            q.ScheduleJob<INodeAccessService>(trigger => trigger
            .WithIdentity("1")
            .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(7)))
            .WithDailyTimeIntervalSchedule(x => x.WithInterval(1, IntervalUnit.Hour))
            .WithDescription("Flushing node access to database"));
            q.ScheduleJob<IRemoveExpiredRolesService>(trigger => trigger
            .WithIdentity("2")
            .StartAt(DateBuilder.EvenSecondDate(DateTimeOffset.UtcNow.AddSeconds(15)))
            .WithDailyTimeIntervalSchedule(x => x.WithInterval(12, IntervalUnit.Hour))
            .WithDescription("Removing expired roles"));

        });
        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.AddInMemoryRateLimiting();

        builder.Services.AddScoped<CircuitHandler, CircuitHandlerService>();

        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        builder.Services.AddQuartzServer(options => {
            options.WaitForJobsToComplete = true;
        });

        builder.Services.AddApplicationInsightsTelemetry();

        var app = builder.Build();

        if (!app.Environment.IsDevelopment()) {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();

        }

        //app.UseRateLimiter();
        app.UseIpRateLimiting();

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

        app.MapControllers()
            ; 
        app.MapBlazorHub()
            ;
        app.MapFallbackToPage("/_Host")
            ;

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}")
            ; 

        //app.MapControllerRoute(
        //   name: "all-else",
        //   pattern: "{*url}",
        //   defaults: new { controller = "Home", action = "AllElse" });

        var res = app.Services.GetService<ISiteDataService>();
        if (res != null) {
            if(await res.InitializeAsync()) {
                app.Run();
            }
        }
    }
}
public static class Extensions
{
    public static AuthenticationBuilder AddProviders(this AuthenticationBuilder builder, List<OAuthProvider> providers)
    {
        foreach(var provider in providers) {
            if(provider.Name == "Google") {
                builder.AddGoogle(options => {
                    options.ClientId = provider.ClientId;
                    options.ClientSecret = provider.ClientSecret;
                });
            }
            if (provider.Name == "Microsoft") {
                builder.AddMicrosoftAccount(options => {
                    options.ClientId = provider.ClientId;
                    options.ClientSecret = provider.ClientSecret;
                });
            }
            if (provider.Name == "Yahoo") {
                builder.AddYahoo(options => {
                    options.ClientId = provider.ClientId;
                    options.ClientSecret = provider.ClientSecret;
                });
            }
        }
        return builder;
    }

    public static IJsonTypeInfoResolver[] GetResolvers()
    {
        return [
            Models.CreateOptionJsonContext.Default,
            Models.SubgroupJsonContext.Default,
            Models.ListOptionJsonContext.Default,
            Models.MenuItemJsonContext.Default,
            Models.NodeAccessJsonContext.Default,
            Models.TenantJsonContext.Default,
            Models.TenantNodeJsonContext.Default,
            Models.UserJsonContext.Default,
            Models.UserRolesToAssignJsonContext.Default,
            Models.UserProfileJsonContext.Default,

            ViewModel.Models.AbuseCaseJsonContext.Default,
            ViewModel.Models.AbuseCaseListJsonContext.Default,
            ViewModel.Models.AbuseCasesJsonContext.Default,
            ViewModel.Models.ArticleListEntryJsonContext.Default,
            ViewModel.Models.ArticleListJsonContext.Default,
            ViewModel.Models.AuthoringJsonContext.Default,
            ViewModel.Models.BasicCountryJsonContext.Default,
            ViewModel.Models.BasicListEntryJsonContext.Default,
            ViewModel.Models.BasicNameableJsonContext.Default,
            ViewModel.Models.BasicPollQuestionJsonContext.Default,
            ViewModel.Models.BillActionJsonContext.Default,
            ViewModel.Models.BillActionTypeJsonContext.Default,
            ViewModel.Models.BindingCountryJsonContext.Default,
            ViewModel.Models.BlogJsonContext.Default,
            ViewModel.Models.BlogEntryListJsonContext.Default,
            ViewModel.Models.BlogPostJsonContext.Default,
            ViewModel.Models.BlogPostTeaserJsonContext.Default,
            ViewModel.Models.BoundCountryJsonContext.Default,
            ViewModel.Models.CaseTeaserListEntryJsonContext.Default,
            ViewModel.Models.CasePartiesJsonContext.Default,
            ViewModel.Models.CasesJsonContext.Default,
            ViewModel.Models.CaseTypeListEntryJsonContext.Default,
            ViewModel.Models.ChildPlacementTypeJsonContext.Default,
            ViewModel.Models.ChildTraffickingCaseJsonContext.Default,
            ViewModel.Models.ChildTraffickingCaseListJsonContext.Default,
            ViewModel.Models.ChildTraffickingCasesJsonContext.Default,
            ViewModel.Models.CoercedAdoptionCaseJsonContext.Default,
            ViewModel.Models.CoercedAdoptionCaseListJsonContext.Default,
            ViewModel.Models.CoercedAdoptionCasesJsonContext.Default,
            ViewModel.Models.CommentJsonContext.Default,
            ViewModel.Models.CommentListItemJsonContext.Default,
            ViewModel.Models.CongressionalChamberJsonContext.Default,
            ViewModel.Models.CongressionalChamberMeetingsJsonContext.Default,
            ViewModel.Models.CongressionalMeetingChamberJsonContext.Default,
            ViewModel.Models.CongressionalTermJsonContext.Default,
            ViewModel.Models.CountryAndSubdivisionJsonContext.Default,
            ViewModel.Models.CountryListEntryJsonContext.Default,
            ViewModel.Models.DeportationCaseJsonContext.Default,
            ViewModel.Models.DenominationJsonContext.Default,
            ViewModel.Models.DeportationCaseListJsonContext.Default,
            ViewModel.Models.DeportationCasesJsonContext.Default,
            ViewModel.Models.DiscussionJsonContext.Default,
            ViewModel.Models.DisruptedPlacementCaseJsonContext.Default,
            ViewModel.Models.DisruptedPlacementCaseListJsonContext.Default,
            ViewModel.Models.DisruptedPlacementCasesJsonContext.Default,
            ViewModel.Models.DocumentJsonContext.Default,
            ViewModel.Models.DocumentsJsonContext.Default,
            ViewModel.Models.DocumentTypeJsonContext.Default,
            ViewModel.Models.DocumentListEntryJsonContext.Default,
            ViewModel.Models.ErrorViewModelJsonContext.Default,
            ViewModel.Models.ExecutiveCompensationJsonContext.Default,
            ViewModel.Models.FamilySizeJsonContext.Default,
            ViewModel.Models.FathersRightsViolationCaseJsonContext.Default,
            ViewModel.Models.FathersRightsViolationCaseListJsonContext.Default,
            ViewModel.Models.FathersRightsViolationCasesJsonContext.Default,
            ViewModel.Models.FileJsonContext.Default,
            ViewModel.Models.FirstLevelRegionListEntryJsonContext.Default,
            ViewModel.Models.FormalSubdivisionJsonContext.Default,
            ViewModel.Models.GlobalRegionJsonContext.Default,
            ViewModel.Models.HagueStatusJsonContext.Default,
            ViewModel.Models.ImageJsonContext.Default,
            ViewModel.Models.InformalSubdivisionJsonContext.Default,
            ViewModel.Models.InterOrganizationalRelationJsonContext.Default,
            ViewModel.Models.InterOrganizationalRelationTypeJsonContext.Default,
            ViewModel.Models.InterPersonalRelationJsonContext.Default,
            ViewModel.Models.InterPersonalRelationTypeJsonContext.Default,
            ViewModel.Models.LocationJsonContext.Default,
            ViewModel.Models.MemberOfCongressJsonContext.Default,
            ViewModel.Models.MultiQuestionPollJsonContext.Default,
            ViewModel.Models.NonSpecificCaseListEntryJsonContext.Default,
            ViewModel.Models.NameablesJsonContext.Default,
            ViewModel.Models.OrganizationJsonContext.Default,
            ViewModel.Models.OrganizationListEntryJsonContext.Default,
            ViewModel.Models.OrganizationPersonRelationJsonContext.Default,
            ViewModel.Models.OrganizationSearchJsonContext.Default,
            ViewModel.Models.OrganizationsJsonContext.Default,
            ViewModel.Models.OrganizationTypeJsonContext.Default,
            ViewModel.Models.OrganizationTypeWithOrganizationsJsonContext.Default,
            ViewModel.Models.PagedSearchListSettingsJsonContext.Default,
            ViewModel.Models.PagedTermedListSettingsJsonContext.Default,
            ViewModel.Models.PageJsonContext.Default,
            ViewModel.Models.PartyCaseJsonContext.Default,
            ViewModel.Models.PartyCaseTypeJsonContext.Default,
            ViewModel.Models.PartyMembershipJsonContext.Default,
            ViewModel.Models.PartyPoliticalEntityRelationJsonContext.Default,
            ViewModel.Models.PartyPoliticalEntityRelationTypeJsonContext.Default,
            ViewModel.Models.PersonJsonContext.Default,
            ViewModel.Models.PersonListEntryJsonContext.Default,
            ViewModel.Models.PersonOrganizationRelationJsonContext.Default,
            ViewModel.Models.PersonOrganizationRelationTypeJsonContext.Default,
            ViewModel.Models.PersonsJsonContext.Default,
            ViewModel.Models.ProfessionJsonContext.Default,
            ViewModel.Models.PollListEntryJsonContext.Default,
            ViewModel.Models.PollOptionJsonContext.Default,
            ViewModel.Models.PollsJsonContext.Default,
            ViewModel.Models.RecentPostsJsonContext.Default,
            ViewModel.Models.RecentPostListEntryJsonContext.Default,
            ViewModel.Models.SearchResultJsonContext.Default,
            ViewModel.Models.SearchResultListEntryJsonContext.Default,
            ViewModel.Models.SecondLevelRegionListEntryJsonContext.Default,
            ViewModel.Models.SelectionItemJsonContext.Default,
            ViewModel.Models.SingleQuestionPollJsonContext.Default,
            ViewModel.Models.StateRepresentationJsonContext.Default,
            ViewModel.Models.SubdivisionListEntryJsonContext.Default,
            ViewModel.Models.SubdivisionTypeJsonContext.Default,
            ViewModel.Models.SubgroupListEntryJsonContext.Default,
            ViewModel.Models.SubgroupPagedListJsonContext.Default,
            ViewModel.Models.TagListEntryJsonContext.Default,
            ViewModel.Models.TopicListEntryJsonContext.Default,
            ViewModel.Models.TopicsJsonContext.Default,
            ViewModel.Models.TypeOfAbuseJsonContext.Default,
            ViewModel.Models.TypeOfAbuserJsonContext.Default,
            ViewModel.Models.UnitedStatesCongressJsonContext.Default,
            ViewModel.Models.WrongfulMedicationCaseJsonContext.Default,
            ViewModel.Models.WrongfulMedicationCaseListJsonContext.Default,
            ViewModel.Models.WrongfulMedicationCasesJsonContext.Default,
            ViewModel.Models.WrongfulRemovalCaseJsonContext.Default,
            ViewModel.Models.WrongfulRemovalCaseListJsonContext.Default,
            ViewModel.Models.WrongfulRemovalCasesJsonContext.Default,

            EditModel.AbuseCaseToUpdateJsonContext.Default,
            EditModel.AbuseCaseToCreateJsonContext.Default,
            EditModel.BasicNameableJsonContext.Default,
            EditModel.BlogPostToUpdateJsonContext.Default,
            EditModel.BlogPostToCreateJsonContext.Default,
            EditModel.ChildPlacementTypeJsonContext.Default,
            EditModel.ChildTraffickingCaseToUpdateJsonContext.Default,
            EditModel.ChildTraffickingCaseToCreateJsonContext.Default,
            EditModel.CoercedAdoptionCaseJsonToUpdateContext.Default,
            EditModel.CoercedAdoptionCaseToCreateJsonContext.Default,
            EditModel.CountryListItemJsonContext.Default,
            EditModel.DenominationJsonContext.Default,
            EditModel.DeportationCaseToUpdateJsonContext.Default,
            EditModel.DeportationCaseToCreateJsonContext.Default,
            EditModel.ExistingDiscussionToUpdateJsonContext.Default,
            EditModel.DiscussionToCreateJsonContext.Default,
            EditModel.DisruptedPlacementCaseToUpdateJsonContext.Default,
            EditModel.DisruptedPlacementCaseToCreateJsonContext.Default,
            EditModel.DocumentToUpdateJsonContext.Default,
            EditModel.DocumentToCreateJsonContext.Default,
            EditModel.DocumentListItemJsonContext.Default,
            EditModel.DocumentTypeJsonContext.Default,
            EditModel.DocumentTypeListItemJsonContext.Default,
            EditModel.FathersRightsViolationCaseToUpdateJsonContext.Default,
            EditModel.FathersRightsViolationToCreateCaseJsonContext.Default,
            EditModel.FileJsonContext.Default,
            EditModel.GeographicalEntityListItemJsonContext.Default,
            EditModel.HagueStatusJsonContext.Default,
            EditModel.InterOrganizationalRelationTypeJsonContext.Default,
            EditModel.InterPersonalRelationTypeJsonContext.Default,
            EditModel.LocationJsonContext.Default,
            EditModel.OrganizationCasePartyJsonContext.Default,
            EditModel.OrganizationTypeJsonContext.Default,
            EditModel.OrganizationToUpdateJsonContext.Default,
            EditModel.OrganizationToCreateJsonContext.Default,
            EditModel.OrganizationListItemJsonContext.Default,
            EditModel.OrganizationNameJsonContext.Default,
            EditModel.OrganizationTypeListItemJsonContext.Default,
            EditModel.OrganizationPoliticalEntityRelationTypeListItemJsonContext.Default,
            EditModel.PartyPoliticalEntityRelationTypeJsonContext.Default,
            EditModel.PersonListItemJsonContext.Default,
            EditModel.PersonOrganizationRelationTypeJsonContext.Default,
            EditModel.PersonPoliticalEntityRelationTypeListItemJsonContext.Default,
            EditModel.PersonToUpdateJsonContext.Default,
            EditModel.PersonToCreateJsonContext.Default,
            EditModel.PersonNameJsonContext.Default,
            EditModel.PersonCasePartyJsonContext.Default,
            EditModel.ProfessionJsonContext.Default,
            EditModel.PersonOrganizationRelationTypeListItemJsonContext.Default,
            EditModel.PoliticalEntityListItemJsonContext.Default,
            EditModel.SubdivisionListItemJsonContext.Default,
            EditModel.SubgroupJsonContext.Default,
            EditModel.TagNodeTypeJsonContext.Default,
            EditModel.TypeOfAbuseJsonContext.Default,
            EditModel.TypeOfAbuserJsonContext.Default,
            EditModel.ExistingTenantNodeJsonContext.Default,
            EditModel.TermJsonContext.Default,
            EditModel.WrongfulMedicationCaseToUpdateJsonContext.Default,
            EditModel.WrongfulMedicationCaseToCreateJsonContext.Default,
            EditModel.WrongfulRemovalToUpdateCaseJsonContext.Default,
            EditModel.WrongfulRemovalToCreateCaseJsonContext.Default,
            EditModel.ChildPlacementTypeListItemJsonContext.Default,
            EditModel.FamilySizeJsonContext.Default,
            EditModel.TypeOfAbuseListItemJsonContext.Default,
            EditModel.TypeOfAbuserListItemJsonContext.Default,
            EditModel.OrganizationListItemJsonContext.Default,
            EditModel.PersonListItemJsonContext.Default,

            Admin.View.TenantJsonContext.Default
        ];

    }
}

