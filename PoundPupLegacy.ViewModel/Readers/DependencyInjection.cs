using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.ViewModel.Readers;

public static class DependencyInjection
{
    public static void AddViewModelReaders(this IServiceCollection services)
    {
        services.AddTransient<ISingleItemDatabaseReaderFactory<ArticlesDocumentReaderRequest, Articles>, ArticlesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<BlogDocumentReaderRequest, Blog>, BlogDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<BlogsDocumentReaderRequest, List<BlogListEntry>>, BlogsDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<CasesDocumentReaderRequest, Cases>, CasesDocumentReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<CountriesDocumentReaderRequest, FirstLevelRegionListEntry[]>, CountriesDocumentReaderFactory>();
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
    }
}
