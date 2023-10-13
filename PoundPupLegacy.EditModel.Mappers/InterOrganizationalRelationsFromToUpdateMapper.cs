namespace PoundPupLegacy.EditModel.Mappers;

internal class InterOrganizationalRelationsFromToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper
) : IEnumerableMapper<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate, DomainModel.InterOrganizationalRelation.ToUpdate>
{
    public IEnumerable<DomainModel.InterOrganizationalRelation.ToUpdate> Map(IEnumerable<InterOrganizationalRelation.From.Complete.Resolved.ToUpdate> source)
    {
        foreach (var relation in source) {
            if (relation.RelationDetails.HasBeenDeleted)
                continue;
            relation.NodeDetails.Title = $"{relation.OrganizationFromName} {relation.InterOrganizationalRelationDetails.InterOrganizationalRelationType.Name.ToLower()} {relation.OrganizationToName}";
            yield return new DomainModel.InterOrganizationalRelation.ToUpdate {
                Identification = new Identification.Certain {
                    Id = relation.NodeIdentification.NodeId,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForUpdate),
                OrganizationIdFrom = relation.OrganizationFrom.Id,
                OrganizationIdTo = relation.OrganizationTo.Id,
                InterOrganizationalRelationDetails = new DomainModel.InterOrganizationalRelationDetails {
                    InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationDetails.InterOrganizationalRelationType.Id,
                    DateRange = relation.RelationDetails.DateRange ?? new DateTimeRange(null, null),
                    GeographicalEntityId = relation.InterOrganizationalRelationDetails.GeographicalEntity?.Id,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    MoneyInvolved = relation.InterOrganizationalRelationDetails.MoneyInvolved,
                    NumberOfChildrenInvolved = relation.InterOrganizationalRelationDetails.NumberOfChildrenInvolved,
                    Description = relation.RelationDetails.Description,
                }
            };
        }
    }
}
