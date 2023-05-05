using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.ViewModel.Models;
using File = PoundPupLegacy.ViewModel.Models.File;

namespace PoundPupLegacy.ViewModel.Readers;

public static class DependencyInjection
{
    public static void AddViewModelReaders(this IServiceCollection services)
    {
        services.AddTransient<ISingleItemDatabaseReaderFactory<AbuseCasesDocumentReaderRequest, AbuseCases>, AbuseCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<BlogDocumentReaderRequest, Blog>, BlogDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<BlogsDocumentReaderRequest, List<BlogListEntry>>, BlogsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<CasesDocumentReaderRequest, Cases>, CasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<ChildTraffickingCasesDocumentReaderRequest, ChildTraffickingCases>, ChildTraffickingCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<CoercedAdoptionCasesDocumentReaderRequest, CoercedAdoptionCases>, CoercedAdoptionCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<CountriesDocumentReaderRequest, FirstLevelRegionListEntry[]>, CountriesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<DeportationCasesDocumentReaderRequest, DeportationCases>, DeportationCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<DisruptedPlacementCasesDocumentReaderRequest, DisruptedPlacementCases>, DisruptedPlacementCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<DocumentsDocumentReaderRequest, Documents>, DocumentsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<FathersRightsViolationCasesDocumentReaderRequest, FathersRightsViolationCases>, FathersRightsViolationCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<FileDocumentReaderRequest, File>, FileDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<NodeDocumentReaderRequest, Node>, NodeDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<OrganizationsDocumentReaderRequest, OrganizationSearch>, OrganizationsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<PersonsDocumentReaderRequest, Persons>, PersonsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<PollsDocumentReaderRequest, Polls>, PollsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<SearchDocumentReaderRequest, SearchResult>, SearchDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<SubgroupsDocumentReaderRequest, SubgroupPagedList>, SubgroupsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<TopicsDocumentReaderRequest, Topics>, TopicsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<UnitedStatesCongresssDocumentReaderRequest, UnitedStatesCongress>, UnitedStatesCongresssDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<UnitedStatesMeetingChamberDocumentReaderRequest, CongressionalMeetingChamber>, UnitedStatesMeetingChamberDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<WrongfulMedicationCasesDocumentReaderRequest, WrongfulMedicationCases>, WrongfulMedicationCasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<WrongfulRemovalCasesDocumentReaderRequest, WrongfulRemovalCases>, WrongfulRemovalCasesDocumentReaderFactory>();
    }
}
