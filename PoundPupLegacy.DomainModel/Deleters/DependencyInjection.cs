using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.DomainModel.Deleters;

public static class DependencyInjection
{
    public static void AddDomainModelDeleters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseDeleterFactory<FileDeleterRequest>, FileDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<NodeTermToRemove>, NodeTermDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<TenantNodeToDelete>, TenantNodeDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<TermHierarchyToRemoveRequest>, TermHierarchyDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<LocationToDelete>, LocationDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<OrganizationOrganizationTypeDeleterRequest>, OrganizationOrganizationTypeDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<CasePartiesOrganizationDeleterRequest>, CasePartiesOrganizationDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<CasePartiesPersonDeleterRequest>, CasePartiesPersonDeleterFactory>();
    }
}
