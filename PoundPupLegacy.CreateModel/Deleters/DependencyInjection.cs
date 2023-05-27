﻿using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.CreateModel.Deleters;

public static class DependencyInjection
{
    public static void AddCreateModelDeleters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseDeleterFactory<FileDeleterRequest>, FileDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<NodeTermToRemove>, NodeTermDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<ImmediatelyIdentifiableTenantNode>, TenantNodeDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<LocationToDelete>, LocationDeleterFactory>();
        services.AddTransient<IDatabaseDeleterFactory<OrganizationOrganizationTypeDeleterRequest>, OrganizationOrganizationTypeDeleterFactory>();
    }
}
