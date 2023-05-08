using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.EditModel.Updaters;

public static class DependencyInjection
{
    public static void AddEditModelUpdaters(this IServiceCollection services)
    {
        services.AddTransient<IDatabaseUpdaterFactory<AbuseCaseUpdaterRequest>, AbuseCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ChildTraffickingCaseUpdaterRequest>, ChildTraffickingCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<CoercedAdoptionCaseUpdaterRequest>, CoercedAdoptionCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<DeportationCaseUpdaterRequest>, DeportationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<DisruptedPlacementCaseUpdaterRequest>, DisruptedPlacementCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<FathersRightsViolationCaseUpdaterRequest>, FathersRightsViolationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<DocumentUpdaterRequest>, DocumentUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<LocationUpdaterRequest>, LocationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<OrganizationUpdaterRequest>, OrganizationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<PersonUpdaterRequest>, PersonUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<SimpleTextNodeUpdaterRequest>, SimpleTextNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<TenantNodeUpdaterRequest>, TenantNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<WrongfulMedicationCaseUpdaterRequest>, WrongfulMedicationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<WrongfulRemovalCaseUpdaterRequest>, WrongfulRemovalCaseUpdaterFactory>();
    }
}
