namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class InterOrganizationalRelationsFromUpdateMapper(
    IMapper<EditModel.NodeDetails.NodeDetailsForUpdate, CreateModel.NodeDetails.NodeDetailsForUpdate> nodeDetailMapper
) : IEnumerableMapper<ExistingInterOrganizationalRelationTo, CreateModel.InterOrganizationalRelation.InterOrganizationalRelationToCreateForNewOrganizationTo.InterOrganizationalRelationToUpdate>
{
    public IEnumerable<CreateModel.InterOrganizationalRelation.InterOrganizationalRelationToCreateForNewOrganizationTo.InterOrganizationalRelationToUpdate> Map(IEnumerable<ExistingInterOrganizationalRelationTo> source)
    {
        foreach (var relation in source) {
            if (relation.RelationDetails.HasBeenDeleted)
                continue;
            yield return new CreateModel.InterOrganizationalRelation.InterOrganizationalRelationToCreateForNewOrganizationTo.InterOrganizationalRelationToUpdate {
                IdentificationForUpdate = new Identification.IdentificationForUpdate {
                    Id = relation.NodeIdentification.NodeId,
                },
                NodeDetailsForUpdate = nodeDetailMapper.Map(relation.NodeDetailsForUpdate),
                OrganizationIdFrom = relation.OrganizationFrom.Id,
                OrganizationIdTo = relation.OrganizationTo.Id,
                InterOrganizationalRelationDetails = new CreateModel.InterOrganizationalRelationDetails {
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
