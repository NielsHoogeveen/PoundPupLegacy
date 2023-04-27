global using Microsoft.AspNetCore.Components;
global using Microsoft.AspNetCore.WebUtilities;
global using PoundPupLegacy.Common;
global using PoundPupLegacy.ViewModel.Models;
using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.ViewModel.Readers;
using PoundPupLegacy.ViewModel.UI.Services;
using PoundPupLegacy.ViewModel.UI.Services.Implementation;
namespace PoundPupLegacy.ViewModel.UI;

public static class DependencyInjection
{
    public static void AddViewModels(this IServiceCollection services)
    {
        services.AddViewModelReaders();
        services.AddTransient<ICongressionalDataService, CongressionalDataService>();
        services.AddTransient<IFetchAttachmentService, FetchAttachmentService>();
        services.AddTransient<IFetchArticlesService, FetchArticlesService>();
        services.AddTransient<IFetchBlogService, FetchBlogService>();
        services.AddTransient<IFetchBlogsService, FetchBlogsService>();
        services.AddTransient<IFetchCasesService, FetchCasesService>();
        services.AddTransient<IFetchCountriesService, FetchCountriesService>();
        services.AddTransient<IFetchNodeService, FetchNodeService>();
        services.AddTransient<IFetchOrganizationsService, FetchOrganizationsService>();
        services.AddTransient<IFetchPersonService, FetchPersonsService>();
        services.AddTransient<IFetchPollsService, FetchPollsService>();
        services.AddTransient<IFetchSearchService, FetchSearchService>();
        services.AddTransient<IFetchSubgroupService, FetchSubgroupService>();
        services.AddTransient<IFetchTopicsService, FetchTopicsService>();
    }
}
