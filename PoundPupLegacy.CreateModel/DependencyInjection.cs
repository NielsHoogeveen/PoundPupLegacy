using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.CreateModel.Creators;
using PoundPupLegacy.CreateModel.Updater;

namespace PoundPupLegacy.CreateModel;

public static class DependencyInjection
{
    public static void AddCreateModelAccessors(this IServiceCollection services)
    {
        services.AddCreateModelInserters();
        services.AddCreateModelCreators();
        services.AddCreateModelUpdaters();
        services.AddCreateModelReaders();
    }
}
