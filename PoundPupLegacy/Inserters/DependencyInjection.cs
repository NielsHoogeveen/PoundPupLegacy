using PoundPupLegacy.Common;

namespace PoundPupLegacy.Inserters;

public static class DependencyInjection
{
    public static void AddSystemInserters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseInserterFactory<FileInserterFactory.Request>, FileInserterFactory>();
    }
}
