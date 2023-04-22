using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.EditModel.Inserters;

public static class DependencyInjection
{
    public static void AddEditModelInserters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseInserterFactory<FileInserterRequest>, FileInserterFactory>();
        services.AddTransient<IDatabaseInserterFactory<OrganizationOrganizationTypeInserterRequest>, OrganizationOrganizationTypeInserterFactory>();
    }
}
