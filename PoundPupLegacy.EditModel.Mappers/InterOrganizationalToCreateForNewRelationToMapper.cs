namespace PoundPupLegacy.EditModel.Mappers;

internal class InterOrganizationalToCreateForNewRelationToMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper
 ) : IEnumerableMapper<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization, DomainModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationTo>
{
    public IEnumerable<DomainModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationTo> Map(IEnumerable<InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new DomainModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationTo {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                OrganizationIdFrom = relation.OrganizationFrom.Id,
                InterOrganizationalRelationDetails = new DomainModel.InterOrganizationalRelationDetails {
                    GeographicalEntityId = relation.InterOrganizationalRelationDetails.GeographicalEntity?.Id,
                    DateRange = relation.RelationDetails.DateRange is null ? new DateTimeRange(null, null) : relation.RelationDetails.DateRange,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description,
                    MoneyInvolved = relation.InterOrganizationalRelationDetails.MoneyInvolved,
                    NumberOfChildrenInvolved = relation.InterOrganizationalRelationDetails.NumberOfChildrenInvolved,
                    InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationDetails.InterOrganizationalRelationType.Id,
                },
            };
        }
    }
}
