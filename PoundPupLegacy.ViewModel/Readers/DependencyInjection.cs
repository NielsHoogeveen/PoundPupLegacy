using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using File = PoundPupLegacy.ViewModel.Models.File;

namespace PoundPupLegacy.ViewModel.Readers;

public static class DependencyInjection
{
    public static void AddViewModelReaders(this IServiceCollection services)
    {
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, AbuseCase>, AbuseCaseDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<AbuseCasesDocumentReaderRequest, AbuseCases>, AbuseCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, BasicNameable>, BasicNameableDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, BasicCountry>, BasicCountryDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, BindingCountry>, BindingCountryDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<BlogDocumentReaderRequest, Blog>, BlogDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<BlogsDocumentReaderRequest, List<BlogListEntry>>, BlogsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, BlogPost>, BlogPostDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, BoundCountry>, BoundCountryDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<CasesDocumentReaderRequest, Cases>, CasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<ChildTraffickingCasesDocumentReaderRequest, ChildTraffickingCases>, ChildTraffickingCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, ChildTraffickingCase>, ChildTraffickingCaseDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<CoercedAdoptionCasesDocumentReaderRequest, CoercedAdoptionCases>, CoercedAdoptionCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, CoercedAdoptionCase>, CoercedAdoptionCaseDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<CountriesDocumentReaderRequest, FirstLevelRegionListEntry[]>, CountriesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, CountryAndSubdivision>, CountryAndSubdivisionDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<DeportationCasesDocumentReaderRequest, DeportationCases>, DeportationCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, DeportationCase>, DeportationCaseDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, Discussion>, DiscussionDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<DisruptedPlacementCasesDocumentReaderRequest, DisruptedPlacementCases>, DisruptedPlacementCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, DisruptedPlacementCase>, DisruptedPlacementCaseDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<DocumentsDocumentReaderRequest, Documents>, DocumentsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, Document>, DocumentDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<FathersRightsViolationCasesDocumentReaderRequest, FathersRightsViolationCases>, FathersRightsViolationCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, FathersRightsViolationCase>, FathersRightsViolationCaseDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<FileDocumentReaderRequest, File>, FileDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, FormalSubdivision>, FormalSubdivisionDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, GlobalRegion>, GlobalRegionDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, InformalSubdivision>, InformalSubdivisionDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, MultiQuestionPoll>, MultiQuestionPollDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NameablesDocumentReaderRequest, Nameables>, NameableListDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, Node>, NodeDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<OrganizationsDocumentReaderRequest, OrganizationSearch>, OrganizationsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, Organization>, OrganizationDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<PersonsDocumentReaderRequest, Persons>, PersonsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, Person>, PersonDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, Page>, PageDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<PollsDocumentReaderRequest, Polls>, PollsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<RecentPostsDocumentReaderRequest, RecentPosts>, RecentPostsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<RecentUserPostsDocumentReaderRequest, RecentPosts>, RecentUserPostsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<SearchDocumentReaderRequest, SearchResult>, SearchDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, SingleQuestionPoll>, SingleQuestionPollDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<SubgroupsDocumentReaderRequest, SubgroupPagedList>, SubgroupsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<TopicsDocumentReaderRequest, Topics>, TopicsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<UnitedStatesCongresssDocumentReaderRequest, UnitedStatesCongress>, UnitedStatesCongresssDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<UnitedStatesMeetingChamberDocumentReaderRequest, CongressionalMeetingChamber>, UnitedStatesMeetingChamberDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<WrongfulMedicationCasesDocumentReaderRequest, WrongfulMedicationCases>, WrongfulMedicationCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, WrongfulMedicationCase>, WrongfulMedicationCaseDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<WrongfulRemovalCasesDocumentReaderRequest, WrongfulRemovalCases>, WrongfulRemovalCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, WrongfulRemovalCase>, WrongfulRemovalCaseDocumentReaderFactory>();
    }
}
