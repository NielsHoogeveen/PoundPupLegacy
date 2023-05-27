using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.EditModel;
using PoundPupLegacy.Services;
using Quartz;
using System.Data;
using System.Text.Json.Serialization.Metadata;

namespace PoundPupLegacy;

public sealed class Program
{
    public static async Task Main(string[] args)
    {
        IJsonTypeInfoResolver[] resolvers = new IJsonTypeInfoResolver[] {
            Models.MenuItemJsonContext.Default,
            Models.TenantJsonContext.Default,
            Models.TenantNodeJsonContext.Default,
            Models.UserTenantActionJsonContext.Default,
            Models.UserTenantEditOwnActionJsonContext.Default,
            Models.UserTenantMenuItemsJsonContext.Default,
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
            ViewModel.Models.BindingCountryJsonContext.Default,
            ViewModel.Models.BlogJsonContext.Default,
            ViewModel.Models.BlogEntryListJsonContext.Default,
            ViewModel.Models.BlogPostJsonContext.Default,
            ViewModel.Models.BlogPostTeaserJsonContext.Default,
            ViewModel.Models.BoundCountryJsonContext.Default,
            ViewModel.Models.CaseListEntryJsonContext.Default,
            ViewModel.Models.CasePartiesJsonContext.Default,
            ViewModel.Models.CasesJsonContext.Default,
            ViewModel.Models.CaseTypeListEntryJsonContext.Default,
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
            ViewModel.Models.DeportationCaseListJsonContext.Default,
            ViewModel.Models.DeportationCasesJsonContext.Default,
            ViewModel.Models.DiscussionJsonContext.Default,
            ViewModel.Models.DisruptedPlacementCaseJsonContext.Default,
            ViewModel.Models.DisruptedPlacementCaseListJsonContext.Default,
            ViewModel.Models.DisruptedPlacementCasesJsonContext.Default,
            ViewModel.Models.DocumentJsonContext.Default,
            ViewModel.Models.DocumentsJsonContext.Default,
            ViewModel.Models.DocumentListEntryJsonContext.Default,
            ViewModel.Models.ErrorViewModelJsonContext.Default,
            ViewModel.Models.ExecutiveCompensationJsonContext.Default,
            ViewModel.Models.FathersRightsViolationCaseJsonContext.Default,
            ViewModel.Models.FathersRightsViolationCaseListJsonContext.Default,
            ViewModel.Models.FathersRightsViolationCasesJsonContext.Default,
            ViewModel.Models.FileJsonContext.Default,
            ViewModel.Models.FirstLevelRegionListEntryJsonContext.Default,
            ViewModel.Models.FormalSubdivisionJsonContext.Default,
            ViewModel.Models.GlobalRegionJsonContext.Default,
            ViewModel.Models.ImageJsonContext.Default,
            ViewModel.Models.InformalSubdivisionJsonContext.Default,
            ViewModel.Models.InterOrganizationalRelationJsonContext.Default,
            ViewModel.Models.InterPersonalRelationJsonContext.Default,
            ViewModel.Models.LocationJsonContext.Default,
            ViewModel.Models.MemberOfCongressJsonContext.Default,
            ViewModel.Models.MultiQuestionPollJsonContext.Default,
            ViewModel.Models.NonSpecificCaseListEntryJsonContext.Default,
            ViewModel.Models.OrganizationJsonContext.Default,
            ViewModel.Models.OrganizationListEntryJsonContext.Default,
            ViewModel.Models.OrganizationPersonRelationJsonContext.Default,
            ViewModel.Models.OrganizationSearchJsonContext.Default,
            ViewModel.Models.OrganizationsJsonContext.Default,
            ViewModel.Models.OrganizationTypeWithOrganizationsJsonContext.Default,
            ViewModel.Models.PagedSearchListSettingsJsonContext.Default,
            ViewModel.Models.PagedTermedListSettingsJsonContext.Default,
            ViewModel.Models.PageJsonContext.Default,
            ViewModel.Models.PartyCaseJsonContext.Default,
            ViewModel.Models.PartyCaseTypeJsonContext.Default,
            ViewModel.Models.PartyMembershipJsonContext.Default,
            ViewModel.Models.PartyPoliticalEntityRelationJsonContext.Default,
            ViewModel.Models.PersonJsonContext.Default,
            ViewModel.Models.PersonListEntryJsonContext.Default,
            ViewModel.Models.PersonOrganizationRelationJsonContext.Default,
            ViewModel.Models.PersonsJsonContext.Default,
            ViewModel.Models.PollListEntryJsonContext.Default,
            ViewModel.Models.PollOptionJsonContext.Default,
            ViewModel.Models.PollsJsonContext.Default,
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
            ViewModel.Models.UnitedStatesCongressJsonContext.Default,
            ViewModel.Models.WrongfulMedicationCaseJsonContext.Default,
            ViewModel.Models.WrongfulMedicationCaseListJsonContext.Default,
            ViewModel.Models.WrongfulMedicationCasesJsonContext.Default,
            ViewModel.Models.WrongfulRemovalCaseJsonContext.Default,
            ViewModel.Models.WrongfulRemovalCaseListJsonContext.Default,
            ViewModel.Models.WrongfulRemovalCasesJsonContext.Default,

            EditModel.ExistingAbuseCaseJsonContext.Default,
            EditModel.NewAbuseCaseJsonContext.Default,
            EditModel.ExistingBlogPostJsonContext.Default,
            EditModel.ExistingChildTraffickingCaseJsonContext.Default,
            EditModel.NewChildTraffickingCaseJsonContext.Default,
            EditModel.ExistingCoercedAdoptionCaseJsonContext.Default,
            EditModel.NewCoercedAdoptionCaseJsonContext.Default,
            EditModel.CountryListItemJsonContext.Default,
            EditModel.ExistingDeportationCaseJsonContext.Default,
            EditModel.NewDeportationCaseJsonContext.Default,
            EditModel.ExistingDiscussionJsonContext.Default,
            EditModel.NewDiscussionJsonContext.Default,
            EditModel.ExistingDisruptedPlacementCaseJsonContext.Default,
            EditModel.NewDisruptedPlacementCaseJsonContext.Default,
            EditModel.ExistingDocumentJsonContext.Default,
            EditModel.NewDocumentJsonContext.Default,
            EditModel.DocumentListItemJsonContext.Default,
            EditModel.DocumentTypeJsonContext.Default,
            EditModel.ExistingFathersRightsViolationCaseJsonContext.Default,
            EditModel.NewFathersRightsViolationCaseJsonContext.Default,
            EditModel.FileJsonContext.Default,
            EditModel.GeographicalEntityListItemJsonContext.Default,
            EditModel.ExistingInterOrganizationalRelationFromJsonContext.Default,
            EditModel.ExistingInterOrganizationalRelationToJsonContext.Default,
            EditModel.ExistingInterPersonalRelationFromJsonContext.Default,
            EditModel.ExistingInterPersonalRelationToJsonContext.Default,
            EditModel.LocationJsonContext.Default,
            EditModel.ExistingOrganizationJsonContext.Default,
            EditModel.NewOrganizationJsonContext.Default,
            EditModel.OrganizationListItemJsonContext.Default,
            EditModel.OrganizationTypeJsonContext.Default,
            EditModel.PersonListItemJsonContext.Default,
            EditModel.ExistingOrganizationPoliticalEntityRelationJsonContext.Default,
            EditModel.OrganizationPoliticalEntityRelationTypeListItemJsonContext.Default,
            EditModel.ExistingPersonPoliticalEntityRelationJsonContext.Default,
            EditModel.PersonPoliticalEntityRelationTypeListItemJsonContext.Default,
            EditModel.ExistingPersonJsonContext.Default,
            EditModel.NewPersonJsonContext.Default,
            EditModel.OrganizationNameJsonContext.Default,
            EditModel.PersonNameJsonContext.Default,
            EditModel.PersonCasePartyJsonContext.Default,
            EditModel.OrganizationCasePartyJsonContext.Default,
            EditModel.ExistingPersonOrganizationRelationForPersonJsonContext.Default,
            EditModel.ExistingPersonOrganizationRelationForOrganizationJsonContext.Default,
            EditModel.PersonOrganizationRelationTypeListItemJsonContext.Default,
            EditModel.PoliticalEntityListItemJsonContext.Default,
            EditModel.SubdivisionListItemJsonContext.Default,
            EditModel.SubgroupJsonContext.Default,
            EditModel.TagNodeTypeJsonContext.Default,
            EditModel.TenantJsonContext.Default,
            EditModel.TenantNodeJsonContext.Default,
            EditModel.TermJsonContext.Default,
            EditModel.ExistingWrongfulMedicationCaseJsonContext.Default,
            EditModel.NewWrongfulMedicationCaseJsonContext.Default,
            EditModel.ExistingWrongfulRemovalCaseJsonContext.Default,
            EditModel.NewWrongfulRemovalCaseJsonContext.Default,
            EditModel.ChildPlacementTypeJsonContext.Default,
            EditModel.FamilySizeJsonContext.Default,
            EditModel.TypeOfAbuseJsonContext.Default,
            EditModel.TypeOfAbuserJsonContext.Default,
            EditModel.OrganizationListItemJsonContext.Default,
            EditModel.PersonListItemJsonContext.Default
        };

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options => {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Forbidden/";

            });

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

        builder.Services.AddRazorPages().AddJsonOptions(options => {
            options
                .JsonSerializerOptions
                .Converters.Add(FuzzyDateJsonConverter.Default);

            options
                .JsonSerializerOptions
                .TypeInfoResolver = JsonTypeInfoResolver.Combine(resolvers);

        });

        builder.Services.AddControllersWithViews().AddJsonOptions(options => {
            options
                .JsonSerializerOptions
                .Converters.Add(FuzzyDateJsonConverter.Default);

            options
                .JsonSerializerOptions
                .TypeInfoResolver = JsonTypeInfoResolver.Combine(resolvers);

        });

        builder.Services.AddServerSideBlazor();
        builder.Services.AddSingleton<NpgsqlDataSource>((sp) => {
            var configuration = sp.GetService<IConfiguration>()!;
            var connectString = configuration["ConnectString"]!;
            var dataSource = new NpgsqlDataSourceBuilder(connectString)
                .UseSystemTextJson(new System.Text.Json.JsonSerializerOptions {
                    TypeInfoResolver = JsonTypeInfoResolver.Combine(resolvers),
                    Converters = { FuzzyDateJsonConverter.Default }
                })
                .Build();
            return dataSource;
        });
        builder.Services.AddTransient<IDbConnection>((sp) => {
            var dataSource = sp.GetRequiredService<NpgsqlDataSource>();
            return dataSource.CreateConnection();
        });
        builder.Services.AddApplicationServices();
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

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        //app.MapControllerRoute(
        //   name: "all-else",
        //   pattern: "{*url}",
        //   defaults: new { controller = "Home", action = "AllElse" });


        var res = app.Services.GetService<ISiteDataService>();
        if (res != null) {
            await res.InitializeAsync();
        }

        app.Run();
    }
}
