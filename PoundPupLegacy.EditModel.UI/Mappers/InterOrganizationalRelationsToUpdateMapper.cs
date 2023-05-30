namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class InterOrganizationalRelationsToUpdateMapper(
    IMapper<EditModel.NodeDetails.NodeDetailsForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper
    ) : IEnumerableMapper<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate, CreateModel.InterOrganizationalRelation.ToUpdate>
{
    public IEnumerable<CreateModel.InterOrganizationalRelation.ToUpdate> Map(IEnumerable<InterOrganizationalRelation.To.Complete.Resolved.ToUpdate> source)
    {
        foreach(var relation in source) {
            if(relation.RelationDetails.HasBeenDeleted)
                continue;
            yield return new CreateModel.InterOrganizationalRelation.ToUpdate {
                Identification = new Identification.Certain {
                    Id = relation.NodeIdentification.NodeId
                },
                NodeDetails = nodeDetailsMapper.Map(relation.NodeDetailsForUpdate),
                InterOrganizationalRelationDetails = new CreateModel.InterOrganizationalRelationDetails {
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
