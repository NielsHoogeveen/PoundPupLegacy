using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.CreateModel.Updaters;

namespace PoundPupLegacy.CreateModel.Updater;

internal static class DependencyInjection
{
    internal static void AddCreateModelUpdaters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseUpdaterFactory<TenantUpdaterSetTaggingVocabulary>, TenantUpdaterSetTaggingVocabularyFactory>();
    }
}
