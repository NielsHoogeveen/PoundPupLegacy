using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.CreateModel.Updaters;

public static class DependencyInjection
{
    public static void AddCreateModelUpdaters(this IServiceCollection services)
    {
        services.AddTransient<IEntityChangerFactory<AbuseCase.ToUpdate>, AbuseCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ChildTraffickingCase.ToUpdate>, ChildTraffickingCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<CoercedAdoptionCase.ToUpdate>, CoercedAdoptionCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<DeportationCase.ToUpdate>, DeportationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<DisruptedPlacementCase.ToUpdate>, DisruptedPlacementCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<FathersRightsViolationCase.FathersRightsViolationCaseToUpdate>, FathersRightsViolationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<Document.DocumentToUpdate>, DocumentChangerFactory>();
        services.AddTransient<IEntityChangerFactory<OrganizationToUpdate>, OrganizationChangerFactory>();
        services.AddTransient<IEntityChangerFactory<Person.ToUpdate>, PersonChangerFactory>();
        services.AddTransient<IEntityChangerFactory<SimpleTextNodeToUpdate>, SimpleTextNodeChangerFactory>();
        services.AddTransient<IEntityChangerFactory<WrongfulMedicationCase.WrongfulMedicationCaseToUpdate>, WrongfulMedicationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<WrongfulRemovalCase.ToUpdate>, WrongfulRemovalCaseChangerFactory>();

        services.AddTransient<IDatabaseUpdaterFactory<AbuseCase.ToUpdate>, AbuseCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ChildTraffickingCase.ToUpdate>, ChildTraffickingCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<CoercedAdoptionCase.ToUpdate>, CoercedAdoptionCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<DeportationCase.ToUpdate>, DeportationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<DisruptedPlacementCase.ToUpdate>, DisruptedPlacementCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<FathersRightsViolationCase.FathersRightsViolationCaseToUpdate>, FathersRightsViolationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Document.DocumentToUpdate>, DocumentUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<LocationUpdaterRequest>, LocationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<InterOrganizationalRelation.ToUpdate>, InterOrganizationalRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<InterPersonalRelation.ToUpdate>, InterPersonalRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<NodeUnpublishRequest>, NodeUnpublishFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<OrganizationToUpdate>, OrganizationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<PartyPoliticalEntityRelation.ToUpdate>, PartyPoliticalEntityRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<PersonOrganizationRelation.ToUpdate>, PersonOrganizationRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Person.ToUpdate>, PersonUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<SimpleTextNodeToUpdate>, SimpleTextNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<TenantNode.ToUpdate>, TenantNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<WrongfulMedicationCase.WrongfulMedicationCaseToUpdate>, WrongfulMedicationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<WrongfulRemovalCase.ToUpdate>, WrongfulRemovalCaseUpdaterFactory>();
    }
}
