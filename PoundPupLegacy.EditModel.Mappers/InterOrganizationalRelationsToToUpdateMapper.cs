namespace PoundPupLegacy.EditModel.Mappers;

internal class InterOrganizationalRelationsToToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailsMapper
    ) : IEnumerableMapper<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate, DomainModel.InterOrganizationalRelation.ToUpdate>
{
    public IEnumerable<DomainModel.InterOrganizationalRelation.ToUpdate> Map(IEnumerable<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate> source)
    {
        foreach (var relation in source) {
            if (relation.RelationDetails.HasBeenDeleted)
                continue;
            yield return new DomainModel.InterOrganizationalRelation.ToUpdate {
                Identification = new Identification.Certain {
                    Id = relation.NodeIdentification.NodeId
                },
                NodeDetails = nodeDetailsMapper.Map(relation.NodeDetailsForUpdate),
                InterOrganizationalRelationDetails = new DomainModel.InterOrganizationalRelationDetails {
                    InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationDetails.InterOrganizationalRelationType.Id,
                    DateRange = relation.RelationDetails.DateRange ?? new DateTimeRange(null, null),
                    GeographicalEntityId = relation.InterOrganizationalRelationDetails.GeographicalEntity?.Id,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    MoneyInvolved = relation.InterOrganizationalRelationDetails.MoneyInvolved,
                    NumberOfChildrenInvolved = relation.InterOrganizationalRelationDetails.NumberOfChildrenInvolved,
                    Description = relation.RelationDetails.Description,
                },
                OrganizationIdFrom = relation.OrganizationFrom.Id,
                OrganizationIdTo = relation.OrganizationTo.Id,
            };
        }
    }
}
