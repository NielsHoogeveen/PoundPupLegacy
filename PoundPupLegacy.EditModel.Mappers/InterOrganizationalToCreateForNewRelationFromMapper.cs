namespace PoundPupLegacy.EditModel.Mappers;

internal class InterOrganizationalToCreateForNewRelationFromMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper
 ) : IEnumerableMapper<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization, DomainModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationFrom>
{
    public IEnumerable<DomainModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationFrom> Map(IEnumerable<InterOrganizationalRelation.From.Complete.ToCreateForNewOrganization> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new DomainModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationFrom {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                OrganizationIdTo = relation.OrganizationTo.Id,
                InterOrganizationalRelationDetails = new DomainModel.InterOrganizationalRelationDetails {
                    GeographicalEntityId = relation.InterOrganizationalRelationDetails.GeographicalEntity?.Id,
                    InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationDetails.InterOrganizationalRelationType.Id,
                    DateRange = relation.RelationDetails.DateRange is null ? new DateTimeRange(null, null) : relation.RelationDetails.DateRange,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description,
                    MoneyInvolved = relation.InterOrganizationalRelationDetails.MoneyInvolved,
                    NumberOfChildrenInvolved = relation.InterOrganizationalRelationDetails.NumberOfChildrenInvolved,
                }
            };
        }
    }
}
