﻿global using Microsoft.AspNetCore.Components;
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
        services.AddFetchService<AbuseCase>();
        services.AddFetchService<BasicCountry>();
        services.AddFetchService<BasicNameable>();
        services.AddFetchService<BindingCountry>();
        services.AddFetchService<BlogPost>();
        services.AddFetchService<BoundCountry>();
        services.AddFetchService<ChildTraffickingCase>();
        services.AddFetchService<CoercedAdoptionCase>();
        services.AddFetchService<CountryAndSubdivision>();
        services.AddFetchService<DeportationCase>();
        services.AddFetchService<Discussion>();
        services.AddFetchService<DisruptedPlacementCase>();
        services.AddFetchService<Document>();
        services.AddFetchService<FathersRightsViolationCase>();
        services.AddFetchService<FormalSubdivision>();
        services.AddFetchService<GlobalRegion>();
        services.AddFetchService<InformalSubdivision>();
        services.AddFetchService<MultiQuestionPoll>();
        services.AddFetchService<Organization>();
        services.AddFetchService<Page>();
        services.AddFetchService<Person>();
        services.AddFetchService<GlobalRegion>();
        services.AddFetchService<SingleQuestionPoll>();
        services.AddFetchService<WrongfulMedicationCase>();
        services.AddFetchService<WrongfulRemovalCase>();
        services.AddTransient<ICongressionalDataService, CongressionalDataService>();
        services.AddTransient<IFetchAbuseCasesService, FetchAbuseCasesService>();
        services.AddTransient<IFetchAttachmentService, FetchAttachmentService>();
        services.AddTransient<IFetchDocumentsService, FetchDocumentsService>();
        services.AddTransient<IFetchBlogService, FetchBlogService>();
        services.AddTransient<IFetchBlogsService, FetchBlogsService>();
        services.AddTransient<IFetchCasesService, FetchCasesService>();
        services.AddTransient<IFetchChildTraffickingCasesService, FetchChildTraffickingCasesService>();
        services.AddTransient<IFetchCoercedAdoptionCasesService, FetchCoercedAdoptionCasesService>();
        services.AddTransient<IFetchCountriesService, FetchCountriesService>();
        services.AddTransient<IFetchNameablesService, FetchNameablesService>();
        services.AddTransient<IFetchDeportationCasesService, FetchDeportationCasesService>();
        services.AddTransient<IFetchDisruptedPlacementCasesService, FetchDisruptedPlacementCasesService>();
        services.AddTransient<IFetchFathersRightsViolationCasesService, FetchFathersRightsViolationCasesService>();
        services.AddTransient<IFetchOrganizationsService, FetchOrganizationsService>();
        services.AddTransient<IFetchPersonService, FetchPersonsService>();
        services.AddTransient<IFetchPollsService, FetchPollsService>();
        services.AddTransient<IFetchRecentPostsService, FetchRecentPostsService>();
        services.AddTransient<IFetchRecentUserPostsService, FetchRecentUserPostsService>();
        services.AddTransient<IFetchSearchService, FetchSearchService>();
        services.AddTransient<IFetchSubgroupService, FetchSubgroupService>();
        services.AddTransient<IFetchTopicsService, FetchTopicsService>();
        services.AddTransient<IFetchWrongfulMedicationCasesService, FetchWrongfulMedicationCasesService>();
        services.AddTransient<IFetchWrongfulRemovalCasesService, FetchWrongfulRemovalCasesService>();
    }

    private static void AddFetchService<T>(this IServiceCollection services)
        where T: class, Node
    {
        services.AddTransient<IFetchNodeService<T>, FetchNodeService<T>>();
    }
}
