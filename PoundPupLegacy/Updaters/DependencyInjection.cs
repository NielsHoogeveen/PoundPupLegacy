using Org.BouncyCastle.Asn1.Ocsp;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.ViewModel.Readers;

namespace PoundPupLegacy.Updaters;

public static class DependencyInjection
{
    public static void AddSystemUpdaters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseUpdaterFactory<UserNameIdentifierUpdaterRequest>, UserNameIdentifierUpdaterFactory>();
    }
}
