using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.CreateModel.Updaters;

public static class DependencyInjection
{
    public static void AddCreateModelUpdaters(this IServiceCollection services)
    {
        services.AddTransient<IEntityChangerFactory<ImmediatelyIdentifiableAbuseCase>, AbuseCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ImmediatelyIdentifiableChildTraffickingCase>, ChildTraffickingCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ImmediatelyIdentifiableCoercedAdoptionCase>, CoercedAdoptionCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ImmediatelyIdentifiableDeportationCase>, DeportationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ImmediatelyIdentifiableDisruptedPlacementCase>, DisruptedPlacementCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ImmediatelyIdentifiableFathersRightsViolationCase>, FathersRightsViolationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ImmediatelyIdentifiableDocument>, DocumentChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ImmediatelyIdentifiableOrganization>, OrganizationChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ImmediatelyIdentifiablePerson>, PersonChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ImmediatelyIdentifiableSimpleTextNode>, SimpleTextNodeChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ImmediatelyIdentifiableWrongfulMedicationCase>, WrongfulMedicationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ImmediatelyIdentifiableWrongfulRemovalCase>, WrongfulRemovalCaseChangerFactory>();

        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableAbuseCase>, AbuseCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableChildTraffickingCase>, ChildTraffickingCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableCoercedAdoptionCase>, CoercedAdoptionCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableDeportationCase>, DeportationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableDisruptedPlacementCase>, DisruptedPlacementCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableFathersRightsViolationCase>, FathersRightsViolationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableDocument>, DocumentUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<LocationUpdaterRequest>, LocationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableInterOrganizationalRelation>, InterOrganizationalRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableInterPersonalRelation>, InterPersonalRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<NodeUnpublishRequest>, NodeUnpublishFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableOrganization>, OrganizationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiablePartyPoliticalEntityRelation>, PartyPoliticalEntityRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiablePersonOrganizationRelation>, PersonOrganizationRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiablePerson>, PersonUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableSimpleTextNode>, SimpleTextNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableTenantNode>, TenantNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableWrongfulMedicationCase>, WrongfulMedicationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ImmediatelyIdentifiableWrongfulRemovalCase>, WrongfulRemovalCaseUpdaterFactory>();
    }
}
