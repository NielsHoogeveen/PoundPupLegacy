using Microsoft.Extensions.DependencyInjection;
using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.Mappers;

public static class DependencyInjection
{
    public static void AddMappers(this IServiceCollection services)
    {
        services.AddTransient<IMapper<AbuseCase.ToUpdate, CreateModel.AbuseCase.ToUpdate>, AbuseCaseToUpdateMapper>();
        services.AddTransient<IMapper<AbuseCase.ToCreate, CreateModel.AbuseCase.ToCreate>, AbuseCaseToCreateMapper>();
        services.AddTransient<IMapper<AbuseCaseDetails, CreateModel.AbuseCaseDetails>, AbuseCaseDetailsMapper>();

        services.AddTransient<IMapper<BlogPost.ToUpdate, CreateModel.BlogPost.ToUpdate>, BlogPostToUpdateMapper>();
        services.AddTransient<IMapper<BlogPost.ToCreate, CreateModel.BlogPost.ToCreate>, BlogPostToCreateMapper>();

        services.AddTransient<IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForCreate>, CaseDetailsForCreateMapper>();
        services.AddTransient<IMapper<CaseDetails, CreateModel.CaseDetails.CaseDetailsForUpdate>, CaseDetailsForUpdateMapper>();

        services.AddTransient<IMapper<ChildTraffickingCase.ToUpdate, CreateModel.ChildTraffickingCase.ToUpdate>, ChildTraffickingCaseToUpdateMapper>();
        services.AddTransient<IMapper<ChildTraffickingCase.ToCreate, CreateModel.ChildTraffickingCase.ToCreate>, ChildTraffickingCaseToCreateMapper>();

        services.AddTransient<IMapper<CoercedAdoptionCase.ToUpdate, CreateModel.CoercedAdoptionCase.ToUpdate>, CoercedAdoptionCaseToUpdateMapper>();
        services.AddTransient<IMapper<CoercedAdoptionCase.ToCreate, CreateModel.CoercedAdoptionCase.ToCreate>, CoercedAdoptionCaseToCreateMapper>();

        services.AddTransient<IMapper<DeportationCase.ToUpdate, CreateModel.DeportationCase.ToUpdate>, DeportationCaseToUpdateMapper>();
        services.AddTransient<IMapper<DeportationCase.ToCreate, CreateModel.DeportationCase.ToCreate>, DeportationCaseToCreateMapper>();

        services.AddTransient<IMapper<DisruptedPlacementCase.ToUpdate, CreateModel.DisruptedPlacementCase.ToUpdate>, DisruptedPlacementCaseToUpdateMapper>();
        services.AddTransient<IMapper<DisruptedPlacementCase.ToCreate, CreateModel.DisruptedPlacementCase.ToCreate>, DisruptedPlacementCaseToCreateMapper>();

        services.AddTransient<IMapper<Document.ToUpdate, CreateModel.Document.ToUpdate>, DocumentToUpdateMapper>();
        services.AddTransient<IMapper<Document.ToCreate, CreateModel.Document.ToCreate>, DocumentToCreateMapper>();

        services.AddTransient<IMapper<FathersRightsViolationCase.ToUpdate, CreateModel.FathersRightsViolationCase.ToUpdate>, FathersRightsViolationCaseToUpdateMapper>();
        services.AddTransient<IMapper<FathersRightsViolationCase.ToCreate, CreateModel.FathersRightsViolationCase.ToCreate>, FathersRightsViolationCaseToCreateMapper>();

        services.AddTransient<IEnumerableMapper<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate, CreateModel.InterOrganizationalRelation.ToUpdate>, InterOrganizationalRelationsFromToUpdateMapper>();
        services.AddTransient<IEnumerableMapper<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate, CreateModel.InterOrganizationalRelation.ToUpdate>, InterOrganizationalRelationsToToUpdateMapper>();

        services.AddTransient<IEnumerableMapper<InterOrganizationalRelation.From.Complete.Resolved.ToCreate, CreateModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants>, InterOrganizationalToCreateForExistingRelationFromMapper>();
        services.AddTransient<IEnumerableMapper<InterOrganizationalRelation.To.Complete.Resolved.ToCreate, CreateModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants>, InterOrganizationalToCreateForExistingRelationToMapper>();

        services.AddTransient<IEnumerableMapper<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization, CreateModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationFrom>, InterOrganizationalToCreateForNewRelationFromMapper>();
        services.AddTransient<IEnumerableMapper<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization, CreateModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationTo>, InterOrganizationalToCreateForNewRelationToMapper>();

        services.AddTransient<IEnumerableMapper<InterPersonalRelation.From.Complete.Resolved.ToUpdate, CreateModel.InterPersonalRelation.ToUpdate>, InterPersonalRelationsFromToUpdateMapper>();
        services.AddTransient<IEnumerableMapper<InterPersonalRelation.To.Complete.Resolved.ToUpdate, CreateModel.InterPersonalRelation.ToUpdate>, InterPersonalRelationsToToUpdateMapper>();

        services.AddTransient<IEnumerableMapper<InterPersonalRelation.From.Complete.Resolved.ToCreate, CreateModel.InterPersonalRelation.ToCreate.ForExistingParticipants>, InterPersonalToCreateForExistingRelationFromMapper>();
        services.AddTransient<IEnumerableMapper<InterPersonalRelation.To.Complete.Resolved.ToCreate, CreateModel.InterPersonalRelation.ToCreate.ForExistingParticipants>, InterPersonalToCreateForExistingRelationToMapper>();

        services.AddTransient<IEnumerableMapper<InterPersonalRelation.From.Complete.ToCreateForNewPerson, CreateModel.InterPersonalRelation.ToCreate.ForNewPersonFrom>, InterPersonalToCreateForNewRelationFromMapper>();
        services.AddTransient<IEnumerableMapper<InterPersonalRelation.To.Complete.ToCreateForNewPerson, CreateModel.InterPersonalRelation.ToCreate.ForNewPersonTo>, InterPersonalToCreateForNewRelationToMapper>();


        services.AddTransient<IMapper<LocatableDetails.ForUpdate, CreateModel.LocatableDetails.ForUpdate>, LocatableDetailsForUpdateMapper>();
        services.AddTransient<IMapper<LocatableDetails.ForCreate, CreateModel.LocatableDetails.ForCreate>, LocatableDetailsForCreateMapper>();

        services.AddTransient<IEnumerableMapper<Location.ToCreate, CreateModel.Location.ToCreate>, LocationsToCreateMapper>();
        services.AddTransient<IEnumerableMapper<Location.ToUpdate, CreateModel.Location.ToUpdate>, LocationsToUpdateMapper>();
        services.AddTransient<IEnumerableMapper<Location.ToUpdate, int>, LocationsToDeleteMapper>();

        services.AddTransient<IMapper<NameableDetails, CreateModel.NameableDetails.ForUpdate>, NameableDetailsForUpdateMapper>();
        services.AddTransient<IMapper<NameableDetails, CreateModel.NameableDetails.ForCreate>, NameableDetailsForCreateMapper>();

        services.AddTransient<IMapper<NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate>, NodeDetailsForUpdateMapper>();
        services.AddTransient<IMapper<NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate>, NodeDetailsForCreateMapper>();

        services.AddTransient<IMapper<Organization.ToUpdate, OrganizationToUpdate>, OrganizationToUpdateMapper>();
        services.AddTransient<IMapper<Organization.ToCreate, OrganizationToCreate>, OrganizationToCreateMapper>();

        services.AddTransient<IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate, PartyPoliticalEntityRelation.ToUpdate>, OrganizationPoliticalEntityRelationToUpdate>();
        services.AddTransient<IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingOrganization, PartyPoliticalEntityRelation.ToCreate.ForExistingParty>, OrganizationPoliticalEntityRelationToCreateForExistingOrganization>();
        services.AddTransient<IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization, PartyPoliticalEntityRelation.ToCreate.ForNewParty>, OrganizationPoliticalEntityRelationToCreateForNewOrganization>();

        services.AddTransient<IMapper<Person.ToUpdate, CreateModel.Person.ToUpdate>, PersonToUpdateMapper>();
        services.AddTransient<IMapper<Person.ToCreate, CreateModel.Person.ToCreate>, PersonToCreateMapper>();

        services.AddTransient<IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate, CreateModel.PersonOrganizationRelation.ToUpdate>, PersonOrganizationRelationToUpdateForOrganization>();
        services.AddTransient<IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToCreate, CreateModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants>, PersonOrganizationRelationToCreateForExistingOrganization>();
        services.AddTransient<IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.ToCreateForNewOrganization, CreateModel.PersonOrganizationRelation.ToCreate.ForNewOrganization>, PersonOrganizationRelationToCreateForNewOrganization>();

        services.AddTransient<IEnumerableMapper<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToUpdate, CreateModel.PersonOrganizationRelation.ToUpdate>, PersonOrganizationRelationToUpdateForPerson>();
        services.AddTransient<IEnumerableMapper<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate, CreateModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants>, PersonOrganizationRelationToCreateForExistingPerson>();
        services.AddTransient<IEnumerableMapper<PersonOrganizationRelation.ForPerson.Complete.ToCreateForNewPerson, CreateModel.PersonOrganizationRelation.ToCreate.ForNewPerson>, PersonOrganizationRelationToCreateForNewPerson>();

        services.AddTransient<IEnumerableMapper<PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate, PartyPoliticalEntityRelation.ToUpdate>, PersonPoliticalEntityRelationToUpdate>();
        services.AddTransient<IEnumerableMapper<PersonPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingPerson, PartyPoliticalEntityRelation.ToCreate.ForExistingParty>, PersonPoliticalEntityRelationToCreateForExistingPerson>();
        services.AddTransient<IEnumerableMapper<PersonPoliticalEntityRelation.Complete.ToCreateForNewPerson, PartyPoliticalEntityRelation.ToCreate.ForNewParty>, PersonPoliticalEntityRelationToCreateForNewPerson>();

        services.AddTransient<IEnumerableMapper<Tags.ToUpdate, ResolvedNodeTermToAdd>, NodeTermToAddForUpdateMapper>();
        services.AddTransient<IEnumerableMapper<Tags.ToUpdate, NodeTermToRemove>, NodeTermToRemoveMapper>();
        services.AddTransient<IEnumerableMapper<Tags.ToCreate, int>, NodeTermToAddForCreateMapper>();

        services.AddTransient<IEnumerableMapper<TenantNode.ToCreateForExistingNode, CreateModel.TenantNode.ToCreate.ForExistingNode>, TenantNodeToCreateForExistingNodeMapper>();
        services.AddTransient<IEnumerableMapper<TenantNode.ToUpdate, CreateModel.Deleters.TenantNodeToDelete>, TenantNodeToRemoveMapper>();
        services.AddTransient<IEnumerableMapper<TenantNode.ToUpdate, CreateModel.TenantNode.ToUpdate>, TenantNodeToUpdateMapper>();
        services.AddTransient<IEnumerableMapper<TenantNode.ToCreateForNewNode, CreateModel.TenantNode.ToCreate.ForNewNode>, TenantNodesToCreateForNewNodeMapper>();

        services.AddTransient<IMapper<WrongfulMedicationCase.ToUpdate, CreateModel.WrongfulMedicationCase.ToUpdate>, WrongfulMedicationCaseToUpdateMapper>();
        services.AddTransient<IMapper<WrongfulMedicationCase.ToCreate, CreateModel.WrongfulMedicationCase.ToCreate>, WrongfulMedicationCaseToCreateMapper>();

        services.AddTransient<IMapper<WrongfulRemovalCase.ToUpdate, CreateModel.WrongfulRemovalCase.ToUpdate>, WrongfulRemovalCaseToUpdateMapper>();
        services.AddTransient<IMapper<WrongfulRemovalCase.ToCreate, CreateModel.WrongfulRemovalCase.ToCreate>, WrongfulRemovalCaseToCreateMapper>();

    }
}
