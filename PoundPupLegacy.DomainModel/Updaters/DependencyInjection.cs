using Microsoft.Extensions.DependencyInjection;

namespace PoundPupLegacy.DomainModel.Updaters;

public static class DependencyInjection
{
    public static void AddDomainModelUpdaters(this IServiceCollection services)
    {
        services.AddTransient<NodeDetailsChangerFactory>();
        services.AddTransient<CaseDetailsChangerFactory>();
        services.AddTransient<IEntityChangerFactory<AbuseCase.ToUpdate>, AbuseCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<BasicNameable.ToUpdate>, BasicNameableChangerFactory>();
        services.AddTransient<IEntityChangerFactory<BlogPost.ToUpdate>, BlogPostChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ChildPlacementType.ToUpdate>, ChildPlacementTypeChangerFactory>();
        services.AddTransient<IEntityChangerFactory<ChildTraffickingCase.ToUpdate>, ChildTraffickingCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<CoercedAdoptionCase.ToUpdate>, CoercedAdoptionCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<Denomination.ToUpdate>, DenominationChangerFactory>();
        services.AddTransient<IEntityChangerFactory<DeportationCase.ToUpdate>, DeportationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<Discussion.ToUpdate>, DiscussionChangerFactory>();
        services.AddTransient<IEntityChangerFactory<DisruptedPlacementCase.ToUpdate>, DisruptedPlacementCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<FathersRightsViolationCase.ToUpdate>, FathersRightsViolationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<Document.ToUpdate>, DocumentChangerFactory>();
        services.AddTransient<IEntityChangerFactory<DocumentType.ToUpdate>, DocumentTypeChangerFactory>();
        services.AddTransient<IEntityChangerFactory<HagueStatus.ToUpdate>, HagueStatusChangerFactory>();
        services.AddTransient<IEntityChangerFactory<InterOrganizationalRelationType.ToUpdate>, InterOrganizationalRelationTypeChangerFactory>();
        services.AddTransient<IEntityChangerFactory<InterPersonalRelationType.ToUpdate>, InterPersonalRelationTypeChangerFactory>();
        services.AddTransient<IEntityChangerFactory<OrganizationToUpdate>, OrganizationChangerFactory>();
        services.AddTransient<IEntityChangerFactory<OrganizationType.ToUpdate>, OrganizationTypeChangerFactory>();
        services.AddTransient<IEntityChangerFactory<PartyPoliticalEntityRelationType.ToUpdate>, PartyPoliticalEntityRelationTypeChangerFactory>();
        services.AddTransient<IEntityChangerFactory<Person.ToUpdate>, PersonChangerFactory>();
        services.AddTransient<IEntityChangerFactory<PersonOrganizationRelationType.ToUpdate>, PersonOrganizationRelationTypeChangerFactory>();
        services.AddTransient<IEntityChangerFactory<TypeOfAbuse.ToUpdate>, TypeOfAbuseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<TypeOfAbuser.ToUpdate>, TypeOfAbuserChangerFactory>();
        services.AddTransient<IEntityChangerFactory<Profession.ToUpdate>, ProfessionChangerFactory>();
        services.AddTransient<IEntityChangerFactory<WrongfulMedicationCase.ToUpdate>, WrongfulMedicationCaseChangerFactory>();
        services.AddTransient<IEntityChangerFactory<WrongfulRemovalCase.ToUpdate>, WrongfulRemovalCaseChangerFactory>();

        services.AddTransient<IDatabaseUpdaterFactory<AbuseCase.ToUpdate>, AbuseCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<BasicNameable.ToUpdate>, BasicNameableUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<BlogPost.ToUpdate>, BlogPostUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<CaseParties.ToUpdate>, CasePartiesUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ChildPlacementType.ToUpdate>, ChildPlacementTypeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<ChildTraffickingCase.ToUpdate>, ChildTraffickingCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<CoercedAdoptionCase.ToUpdate>, CoercedAdoptionCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Denomination.ToUpdate>, DenominationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<DeportationCase.ToUpdate>, DeportationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Discussion.ToUpdate>, DiscussionUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<DisruptedPlacementCase.ToUpdate>, DisruptedPlacementCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Document.ToUpdate>, DocumentUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<DocumentType.ToUpdate>, DocumentTypeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<FathersRightsViolationCase.ToUpdate>, FathersRightsViolationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<HagueStatus.ToUpdate>, HagueStatusUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<InterOrganizationalRelation.ToUpdate>, InterOrganizationalRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<InterOrganizationalRelationType.ToUpdate>, InterOrganizationalRelationTypeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<InterPersonalRelation.ToUpdate>, InterPersonalRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<InterPersonalRelationType.ToUpdate>, InterPersonalRelationTypeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<LocationUpdaterRequest>, LocationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<NodeDetails.ForUpdate>, NodeDetailsUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<NodeUnpublishRequest>, NodeUnpublishFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<OrganizationToUpdate>, OrganizationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<OrganizationType.ToUpdate>, OrganizationTypeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<PartyPoliticalEntityRelation.ToUpdate>, PartyPoliticalEntityRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<PartyPoliticalEntityRelationType.ToUpdate>, PartyPoliticalEntityRelationTypeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<PersonOrganizationRelation.ToUpdate>, PersonOrganizationRelationUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<PersonOrganizationRelationType.ToUpdate>, PersonOrganizationRelationTypeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Person.ToUpdate>, PersonUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Profession.ToUpdate>, ProfessionUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<TenantNode.ToUpdate>, TenantNodeUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<Term.ToUpdate>, TermUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<TypeOfAbuse.ToUpdate>, TypeOfAbuseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<TypeOfAbuser.ToUpdate>, TypeOfAbuserUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<WrongfulMedicationCase.ToUpdate>, WrongfulMedicationCaseUpdaterFactory>();
        services.AddTransient<IDatabaseUpdaterFactory<WrongfulRemovalCase.ToUpdate>, WrongfulRemovalCaseUpdaterFactory>();
    }
}
