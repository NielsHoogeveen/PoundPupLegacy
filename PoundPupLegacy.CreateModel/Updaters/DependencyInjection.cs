using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.CreateModel.Updaters;

public static class DependencyInjection
{
    public static void AddCreateModelUpdaters(this IServiceCollection services)
    {
        services.AddTransient<IEntityChangerFactory<AbuseCase.AbuseCaseToUpdate>, AbuseCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ChildTraffickingCase.ChildTraffickingCaseToUpdate>, ChildTraffickingCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<CoercedAdoptionCase.CoercedAdoptionCaseToUpdate>, CoercedAdoptionCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<DeportationCase.DeportationCaseToUpdate>, DeportationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<DisruptedPlacementCase.DisruptedPlacementCaseToUpdate>, DisruptedPlacementCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<FathersRightsViolationCase.FathersRightsViolationCaseToUpdate>, FathersRightsViolationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<Document.DocumentToUpdate>, DocumentChangerFactory>();
        services.AddTransient<IEntityChangerFactory<OrganizationToUpdate>, OrganizationChangerFactory>();
        services.AddTransient<IEntityChangerFactory<Person.PersonToUpdate>, PersonChangerFactory>();
        services.AddTransient<IEntityChangerFactory<SimpleTextNodeToUpdate>, SimpleTextNodeChangerFactory>();
        services.AddTransient<IEntityChangerFactory<WrongfulMedicationCase.WrongfulMedicationCaseToUpdate>, WrongfulMedicationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<WrongfulRemovalCase.WrongfulRemovalCaseToUpdate>, WrongfulRemovalCaseChangerFactory>();

        services.AddTransient<IDatabaseUpdaterFactory<AbuseCase.AbuseCaseToUpdate>, AbuseCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ChildTraffickingCase.ChildTraffickingCaseToUpdate>, ChildTraffickingCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<CoercedAdoptionCase.CoercedAdoptionCaseToUpdate>, CoercedAdoptionCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<DeportationCase.DeportationCaseToUpdate>, DeportationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<DisruptedPlacementCase.DisruptedPlacementCaseToUpdate>, DisruptedPlacementCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<FathersRightsViolationCase.FathersRightsViolationCaseToUpdate>, FathersRightsViolationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Document.DocumentToUpdate>, DocumentUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<LocationUpdaterRequest>, LocationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<InterOrganizationalRelation.InterOrganizationalRelationToUpdate>, InterOrganizationalRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<InterPersonalRelation.InterPersonalRelationToUpdate>, InterPersonalRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<NodeUnpublishRequest>, NodeUnpublishFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<OrganizationToUpdate>, OrganizationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<PartyPoliticalEntityRelation.PartyPoliticalEntityRelationToUpdate>, PartyPoliticalEntityRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<PersonOrganizationRelation.PersonOrganizationRelationToUpdate>, PersonOrganizationRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Person.PersonToUpdate>, PersonUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<SimpleTextNodeToUpdate>, SimpleTextNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<TenantNode.TenantNodeToUpdate>, TenantNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<WrongfulMedicationCase.WrongfulMedicationCaseToUpdate>, WrongfulMedicationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<WrongfulRemovalCase.WrongfulRemovalCaseToUpdate>, WrongfulRemovalCaseUpdaterFactory>();
    }
}
