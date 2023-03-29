using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.ViewModel.Readers;

public static class DependencyInjection
{
    public static void AddViewModelReaders(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseReaderFactory<ArticlesDocumentReader>, ArticlesDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<BlogDocumentReader>, BlogDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<BlogsDocumentReader>, BlogsDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<CasesDocumentReader>, CasesDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<CountriesDocumentReader>, CountriesDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<FileDocumentReader>, FileDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<NodeDocumentReader>, NodeDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<OrganizationsDocumentReader>, OrganizationsDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<PersonsDocumentReader>, PersonsDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<PollsDocumentReader>, PollsDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<SearchDocumentReader>, SearchDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<SubgroupsDocumentReader>, SubgroupsDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<TopicsDocumentReader>, TopicsDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<UnitedStatesCongresssDocumentReader>, UnitedStatesCongresssDocumentReaderFactory>();
        services.AddTransient<IDatabaseReaderFactory<UnitedStatesMeetingChamberDocumentReader>, UnitedStatesMeetingChamberDocumentReaderFactory>();
    }
}
