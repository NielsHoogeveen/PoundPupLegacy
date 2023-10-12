using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.DomainModel.Updaters;

public static class DependencyInjection
{
    public static void AddDomainModelUpdaters(this IServiceCollection services)
    {
        services.AddTransient<NodeDetailsChangerFactory>();
        services.AddTransient<CaseDetailsChangerFactory>();
        services.AddTransient<IEntityChangerFactory<AbuseCase.ToUpdate>, AbuseCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<BlogPost.ToUpdate>, BlogPostChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ChildTraffickingCase.ToUpdate>, ChildTraffickingCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<CoercedAdoptionCase.ToUpdate>, CoercedAdoptionCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<DeportationCase.ToUpdate>, DeportationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<Discussion.ToUpdate>, DiscussionChangerFactory>();
        services.AddTransient<IEntityChangerFactory<DisruptedPlacementCase.ToUpdate>, DisruptedPlacementCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<FathersRightsViolationCase.ToUpdate>, FathersRightsViolationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<Document.ToUpdate>, DocumentChangerFactory>();
        services.AddTransient<IEntityChangerFactory<OrganizationToUpdate>, OrganizationChangerFactory>();
        services.AddTransient<IEntityChangerFactory<Person.ToUpdate>, PersonChangerFactory>();
        services.AddTransient<IEntityChangerFactory<WrongfulMedicationCase.ToUpdate>, WrongfulMedicationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<WrongfulRemovalCase.ToUpdate>, WrongfulRemovalCaseChangerFactory>();

        services.AddTransient<IDatabaseUpdaterFactory<AbuseCase.ToUpdate>, AbuseCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<BlogPost.ToUpdate>, BlogPostUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<CaseParties.ToUpdate>, CasePartiesUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ChildTraffickingCase.ToUpdate>, ChildTraffickingCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<CoercedAdoptionCase.ToUpdate>, CoercedAdoptionCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<DeportationCase.ToUpdate>, DeportationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Discussion.ToUpdate>, DiscussionUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<DisruptedPlacementCase.ToUpdate>, DisruptedPlacementCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<FathersRightsViolationCase.ToUpdate>, FathersRightsViolationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Document.ToUpdate>, DocumentUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<LocationUpdaterRequest>, LocationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<NodeDetails.ForUpdate>, NodeDetailsUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<InterOrganizationalRelation.ToUpdate>, InterOrganizationalRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<InterPersonalRelation.ToUpdate>, InterPersonalRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<NodeUnpublishRequest>, NodeUnpublishFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<OrganizationToUpdate>, OrganizationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<PartyPoliticalEntityRelation.ToUpdate>, PartyPoliticalEntityRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<PersonOrganizationRelation.ToUpdate>, PersonOrganizationRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Person.ToUpdate>, PersonUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<TenantNode.ToUpdate>, TenantNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Term.ToUpdate>, TermUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<WrongfulMedicationCase.ToUpdate>, WrongfulMedicationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<WrongfulRemovalCase.ToUpdate>, WrongfulRemovalCaseUpdaterFactory>();
    }
}
