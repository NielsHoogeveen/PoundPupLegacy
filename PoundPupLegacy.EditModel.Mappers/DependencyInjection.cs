using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.DomainModel;
using PoundPupLegacy.DomainModel.Deleters;

namespace PoundPupLegacy.EditModel.Mappers;

public static class DependencyInjection
{
    public static void AddMappers(this IServiceCollection services)
    {
        services.AddTransient<IMapper<AbuseCase.ToUpdate, DomainModel.AbuseCase.ToUpdate>, AbuseCaseToUpdateMapper>();
        services.AddTransient<IMapper<AbuseCase.ToCreate, DomainModel.AbuseCase.ToCreate>, AbuseCaseToCreateMapper>();
        services.AddTransient<IMapper<AbuseCaseDetails, DomainModel.AbuseCaseDetails>, AbuseCaseDetailsMapper>();

        services.AddTransient<IMapper<BlogPost.ToUpdate, DomainModel.BlogPost.ToUpdate>, BlogPostToUpdateMapper>();
        services.AddTransient<IMapper<BlogPost.ToCreate, DomainModel.BlogPost.ToCreate>, BlogPostToCreateMapper>();

        services.AddTransient<IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForCreate>, CaseDetailsForCreateMapper>();
        services.AddTransient<IMapper<CaseDetails, DomainModel.CaseDetails.CaseDetailsForUpdate>, CaseDetailsForUpdateMapper>();

        services.AddTransient<IMapper<ChildTraffickingCase.ToUpdate, DomainModel.ChildTraffickingCase.ToUpdate>, ChildTraffickingCaseToUpdateMapper>();
        services.AddTransient<IMapper<ChildTraffickingCase.ToCreate, DomainModel.ChildTraffickingCase.ToCreate>, ChildTraffickingCaseToCreateMapper>();

        services.AddTransient<IMapper<CoercedAdoptionCase.ToUpdate, DomainModel.CoercedAdoptionCase.ToUpdate>, CoercedAdoptionCaseToUpdateMapper>();
        services.AddTransient<IMapper<CoercedAdoptionCase.ToCreate, DomainModel.CoercedAdoptionCase.ToCreate>, CoercedAdoptionCaseToCreateMapper>();

        services.AddTransient<IMapper<DeportationCase.ToUpdate, DomainModel.DeportationCase.ToUpdate>, DeportationCaseToUpdateMapper>();
        services.AddTransient<IMapper<DeportationCase.ToCreate, DomainModel.DeportationCase.ToCreate>, DeportationCaseToCreateMapper>();

        services.AddTransient<IMapper<Discussion.ToUpdate, DomainModel.Discussion.ToUpdate>, DiscussionToUpdateMapper>();
        services.AddTransient<IMapper<Discussion.ToCreate, DomainModel.Discussion.ToCreate>, DiscussionToCreateMapper>();

        services.AddTransient<IMapper<DisruptedPlacementCase.ToUpdate, DomainModel.DisruptedPlacementCase.ToUpdate>, DisruptedPlacementCaseToUpdateMapper>();
        services.AddTransient<IMapper<DisruptedPlacementCase.ToCreate, DomainModel.DisruptedPlacementCase.ToCreate>, DisruptedPlacementCaseToCreateMapper>();

        services.AddTransient<IMapper<Document.ToUpdate, DomainModel.Document.ToUpdate>, DocumentToUpdateMapper>();
        services.AddTransient<IMapper<Document.ToCreate, DomainModel.Document.ToCreate>, DocumentToCreateMapper>();

        services.AddTransient<IMapper<FathersRightsViolationCase.ToUpdate, DomainModel.FathersRightsViolationCase.ToUpdate>, FathersRightsViolationCaseToUpdateMapper>();
        services.AddTransient<IMapper<FathersRightsViolationCase.ToCreate, DomainModel.FathersRightsViolationCase.ToCreate>, FathersRightsViolationCaseToCreateMapper>();

        services.AddTransient<IEnumerableMapper<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate, DomainModel.InterOrganizationalRelation.ToUpdate>, InterOrganizationalRelationsFromToUpdateMapper>();
        services.AddTransient<IEnumerableMapper<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate, DomainModel.InterOrganizationalRelation.ToUpdate>, InterOrganizationalRelationsToToUpdateMapper>();

        services.AddTransient<IEnumerableMapper<InterOrganizationalRelation.From.Complete.Resolved.ToCreate, DomainModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants>, InterOrganizationalToCreateForExistingRelationFromMapper>();
        services.AddTransient<IEnumerableMapper<InterOrganizationalRelation.To.Complete.Resolved.ToCreate, DomainModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants>, InterOrganizationalToCreateForExistingRelationToMapper>();

        services.AddTransient<IEnumerableMapper<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization, DomainModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationFrom>, InterOrganizationalToCreateForNewRelationFromMapper>();
        services.AddTransient<IEnumerableMapper<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization, DomainModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationTo>, InterOrganizationalToCreateForNewRelationToMapper>();

        services.AddTransient<IEnumerableMapper<InterPersonalRelation.From.Complete.Resolved.ToUpdate, DomainModel.InterPersonalRelation.ToUpdate>, InterPersonalRelationsFromToUpdateMapper>();
        services.AddTransient<IEnumerableMapper<InterPersonalRelation.To.Complete.Resolved.ToUpdate, DomainModel.InterPersonalRelation.ToUpdate>, InterPersonalRelationsToToUpdateMapper>();

        services.AddTransient<IEnumerableMapper<InterPersonalRelation.From.Complete.Resolved.ToCreate, DomainModel.InterPersonalRelation.ToCreate.ForExistingParticipants>, InterPersonalToCreateForExistingRelationFromMapper>();
        services.AddTransient<IEnumerableMapper<InterPersonalRelation.To.Complete.Resolved.ToCreate, DomainModel.InterPersonalRelation.ToCreate.ForExistingParticipants>, InterPersonalToCreateForExistingRelationToMapper>();

        services.AddTransient<IEnumerableMapper<InterPersonalRelation.From.Complete.ToCreateForNewPerson, DomainModel.InterPersonalRelation.ToCreate.ForNewPersonFrom>, InterPersonalToCreateForNewRelationFromMapper>();
        services.AddTransient<IEnumerableMapper<InterPersonalRelation.To.Complete.ToCreateForNewPerson, DomainModel.InterPersonalRelation.ToCreate.ForNewPersonTo>, InterPersonalToCreateForNewRelationToMapper>();


        services.AddTransient<IMapper<LocatableDetails.ForUpdate, DomainModel.LocatableDetails.ForUpdate>, LocatableDetailsForUpdateMapper>();
        services.AddTransient<IMapper<LocatableDetails.ForCreate, DomainModel.LocatableDetails.ForCreate>, LocatableDetailsForCreateMapper>();

        services.AddTransient<IEnumerableMapper<Location.ToCreate, DomainModel.Location.ToCreate>, LocationsToCreateMapper>();
        services.AddTransient<IEnumerableMapper<Location.ToUpdate, DomainModel.Location.ToUpdate>, LocationsToUpdateMapper>();
        services.AddTransient<IEnumerableMapper<Location.ToUpdate, int>, LocationsToDeleteMapper>();

        services.AddTransient<IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate>, NameableDetailsForUpdateMapper>();
        services.AddTransient<IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate>, NameableDetailsForCreateMapper>();

        services.AddTransient<IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate>, NodeDetailsForUpdateMapper>();
        services.AddTransient<IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate>, NodeDetailsForCreateMapper>();

        services.AddTransient<IMapper<Organization.ToUpdate, OrganizationToUpdate>, OrganizationToUpdateMapper>();
        services.AddTransient<IMapper<Organization.ToCreate, OrganizationToCreate>, OrganizationToCreateMapper>();

        services.AddTransient<IMapper<OrganizationType.ToUpdate, DomainModel.OrganizationType.ToUpdate>, OrganizationTypeToUpdateMapper>();
        services.AddTransient<IMapper<OrganizationType.ToCreate, DomainModel.OrganizationType.ToCreate>, OrganizationTypeToCreateMapper>();

        services.AddTransient<IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate, PartyPoliticalEntityRelation.ToUpdate>, OrganizationPoliticalEntityRelationToUpdate>();
        services.AddTransient<IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingOrganization, PartyPoliticalEntityRelation.ToCreate.ForExistingParty>, OrganizationPoliticalEntityRelationToCreateForExistingOrganization>();
        services.AddTransient<IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization, PartyPoliticalEntityRelation.ToCreate.ForNewParty>, OrganizationPoliticalEntityRelationToCreateForNewOrganization>();

        services.AddTransient<IMapper<Person.ToUpdate, DomainModel.Person.ToUpdate>, PersonToUpdateMapper>();
        services.AddTransient<IMapper<Person.ToCreate, DomainModel.Person.ToCreate>, PersonToCreateMapper>();

        services.AddTransient<IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate, DomainModel.PersonOrganizationRelation.ToUpdate>, PersonOrganizationRelationToUpdateForOrganization>();
        services.AddTransient<IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToCreate, DomainModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants>, PersonOrganizationRelationToCreateForExistingOrganization>();
        services.AddTransient<IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.ToCreateForNewOrganization, DomainModel.PersonOrganizationRelation.ToCreate.ForNewOrganization>, PersonOrganizationRelationToCreateForNewOrganization>();

        services.AddTransient<IEnumerableMapper<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToUpdate, DomainModel.PersonOrganizationRelation.ToUpdate>, PersonOrganizationRelationToUpdateForPerson>();
        services.AddTransient<IEnumerableMapper<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate, DomainModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants>, PersonOrganizationRelationToCreateForExistingPerson>();
        services.AddTransient<IEnumerableMapper<PersonOrganizationRelation.ForPerson.Complete.ToCreateForNewPerson, DomainModel.PersonOrganizationRelation.ToCreate.ForNewPerson>, PersonOrganizationRelationToCreateForNewPerson>();

        services.AddTransient<IEnumerableMapper<PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate, PartyPoliticalEntityRelation.ToUpdate>, PersonPoliticalEntityRelationToUpdate>();
        services.AddTransient<IEnumerableMapper<PersonPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingPerson, PartyPoliticalEntityRelation.ToCreate.ForExistingParty>, PersonPoliticalEntityRelationToCreateForExistingPerson>();
        services.AddTransient<IEnumerableMapper<PersonPoliticalEntityRelation.Complete.ToCreateForNewPerson, PartyPoliticalEntityRelation.ToCreate.ForNewParty>, PersonPoliticalEntityRelationToCreateForNewPerson>();

        services.AddTransient<IEnumerableMapper<Tags.ToUpdate, ResolvedNodeTermToAdd>, NodeTermToAddForUpdateMapper>();
        services.AddTransient<IEnumerableMapper<Tags.ToUpdate, NodeTermToRemove>, NodeTermToRemoveMapper>();
        services.AddTransient<IEnumerableMapper<Tags.ToCreate, int>, NodeTermToAddForCreateMapper>();

        services.AddTransient<IEnumerableMapper<TenantNode.ToCreateForExistingNode, DomainModel.TenantNode.ToCreate.ForExistingNode>, TenantNodeToCreateForExistingNodeMapper>();
        services.AddTransient<IEnumerableMapper<TenantNode.ToUpdate, TenantNodeToDelete>, TenantNodeToRemoveMapper>();
        services.AddTransient<IEnumerableMapper<TenantNode.ToUpdate, DomainModel.TenantNode.ToUpdate>, TenantNodeToUpdateMapper>();
        services.AddTransient<IEnumerableMapper<TenantNode.ToCreateForNewNode, DomainModel.TenantNode.ToCreate.ForNewNode>, TenantNodesToCreateForNewNodeMapper>();

        services.AddTransient<IMapper<WrongfulMedicationCase.ToUpdate, DomainModel.WrongfulMedicationCase.ToUpdate>, WrongfulMedicationCaseToUpdateMapper>();
        services.AddTransient<IMapper<WrongfulMedicationCase.ToCreate, DomainModel.WrongfulMedicationCase.ToCreate>, WrongfulMedicationCaseToCreateMapper>();

        services.AddTransient<IMapper<WrongfulRemovalCase.ToUpdate, DomainModel.WrongfulRemovalCase.ToUpdate>, WrongfulRemovalCaseToUpdateMapper>();
        services.AddTransient<IMapper<WrongfulRemovalCase.ToCreate, DomainModel.WrongfulRemovalCase.ToCreate>, WrongfulRemovalCaseToCreateMapper>();

    }
}
