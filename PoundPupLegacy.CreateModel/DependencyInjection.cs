using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.DomainModel.Creators;
using PoundPupLegacy.DomainModel.Inserters;
using PoundPupLegacy.DomainModel.Readers;

namespace PoundPupLegacy.DomainModel;

public static class DependencyInjection
{
    public static void AddDomainModelAccessors(this IServiceCollection services)
    {
        services.AddDomainModelInserters();
        services.AddDomainModelCreators();
        services.AddDomainModelReaders();
    }
}
