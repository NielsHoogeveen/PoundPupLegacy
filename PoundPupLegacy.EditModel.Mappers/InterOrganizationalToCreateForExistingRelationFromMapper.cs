namespace PoundPupLegacy.EditModel.Mappers;

internal class InterOrganizationalToCreateForExistingRelationFromMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper
) : IEnumerableMapper<InterOrganizationalRelation.From.Complete.Resolved.ToCreate, DomainModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants>
{
    public IEnumerable<DomainModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants> Map(IEnumerable<InterOrganizationalRelation.From.Complete.Resolved.ToCreate> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new DomainModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                OrganizationIdFrom = relation.OrganizationFrom.Id,
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
