using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewInterOrganizationalNewRelationFromMapper(
    IEnumerableMapper<TenantNode.NewTenantNodeForNewNode, NewTenantNodeForNewNode> tenantNodeMapper,
    IEnumerableMapper<Tags, int> termIdsMapper
 ) : IEnumerableMapper<NewInterOrganizationalExistingRelationFrom, NewInterOrganizationalRelationForNewOrganizationFrom>
{
    public IEnumerable<NewInterOrganizationalRelationForNewOrganizationFrom> Map(IEnumerable<NewInterOrganizationalExistingRelationFrom> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new NewInterOrganizationalRelationForNewOrganizationFrom {
                Id = null,
                PublisherId = relation.NodeDetails.PublisherId,
                CreatedDateTime = now,
                ChangedDateTime = now,
                Title = relation.NodeDetails.Title,
                OwnerId = relation.NodeDetails.OwnerId,
                AuthoringStatusId = 1,
                TenantNodes = tenantNodeMapper.Map(relation.NewTenantNodeDetails.TenantNodesToAdd).ToList(),
                NodeTypeId = 47,
                OrganizationIdTo = relation.OrganizationTo.Id,
                GeographicalEntityId = relation.InterOrganizationalRelationDetails.GeographicalEntity?.Id,
                InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationDetails.InterOrganizationalRelationType.Id,
                DateRange = relation.RelationDetails.DateRange is null ? new DateTimeRange(null, null) : relation.RelationDetails.DateRange,
                DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                Description = relation.RelationDetails.Description,
                MoneyInvolved = relation.InterOrganizationalRelationDetails.MoneyInvolved,
                NumberOfChildrenInvolved = relation.InterOrganizationalRelationDetails.NumberOfChildrenInvolved,
                TermIds = termIdsMapper.Map(relation.NodeDetails.Tags).ToList(),
            };
        }
    }
}
