using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class InterOrganizationalRelationsFromUpdateMapper : IEnumerableMapper<ExistingInterOrganizationalRelationTo, ImmediatelyIdentifiableInterOrganizationalRelation>
{
    public IEnumerable<ImmediatelyIdentifiableInterOrganizationalRelation> Map(IEnumerable<ExistingInterOrganizationalRelationTo> source)
    {
        foreach (var relation in source) {
            if (relation.RelationDetails.HasBeenDeleted)
                continue;
            yield return new ExistingInterOrganizationalRelation {
                Id = relation.NodeIdentification.NodeId,
                Title = relation.NodeDetails.Title,
                Description = relation.RelationDetails.Description,
                OrganizationIdFrom = relation.OrganizationFrom.Id,
                OrganizationIdTo = relation.OrganizationTo.Id,
                InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationDetails.InterOrganizationalRelationType.Id,
                DateRange = relation.RelationDetails.DateRange ?? new DateTimeRange(null, null),
                GeographicalEntityId = relation.InterOrganizationalRelationDetails.GeographicalEntity?.Id,
                DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                MoneyInvolved = relation.InterOrganizationalRelationDetails.MoneyInvolved,
                NumberOfChildrenInvolved = relation.InterOrganizationalRelationDetails.NumberOfChildrenInvolved,
                NodeTermsToAdd = new List<NodeTermToAdd>(),
                NodeTermsToRemove = new List<NodeTermToRemove>(),
                AuthoringStatusId = relation.RelationDetails.HasBeenDeleted ? 2 : 1,
                ChangedDateTime = DateTime.Now,
                TenantNodesToAdd = new List<NewTenantNodeForExistingNode>(),
                TenantNodesToRemove = new List<TenantNodeToDelete>(),
                TenantNodesToUpdate = new List<ExistingTenantNode>()
            };
        }
    }
}
