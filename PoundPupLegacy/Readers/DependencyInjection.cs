﻿using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using PoundPupLegacy.ViewModel.Readers;

namespace PoundPupLegacy.Readers;

public static class DependencyInjection
{
    public static void AddSystemReaders(this IServiceCollection services)
    {
        services.AddTransient<ISingleItemDatabaseReaderFactory<UsersRolesToAsignReaderRequest, List<UserRolesToAssign>>, UsersRolesToAsignReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<CreateOptionsReaderRequest, List<CreateOption>>, CreateOptionsReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<SubgroupsReaderRequest, List<Subgroup>>, SubgroupsReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<ListOptionsReaderRequest, List<ListOption>>, ListOptionsReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<NodeAccessReaderRequest, NodeAccess>, NodeAccessReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<PasswordValidationReaderRequest, PasswordValidationReaderResponse>, PasswordValidationReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<UserByEmailReaderRequest, User>, UserByEmailReaderFactory>();
        services.AddTransient<ISingleItemDatabaseReaderFactory<UserByNameIdentifierReaderRequest, User>, UserByNameIdentifierReaderFactory>();
        services.AddTransient<IEnumerableDatabaseReaderFactory<SiteMapReaderRequest, SiteMapElement>, SiteMapReaderFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<SiteMapCountReaderRequest, SiteMapCount>, SiteMapCountReaderFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<UserDocumentReaderRequest, UserWithDetails>, UserDocumentReaderFactory>();
        services.AddTransient<IMandatorySingleItemDatabaseReaderFactory<TenantReaderRequest, Tenant>, TenantReaderFactory>();
    }
}
