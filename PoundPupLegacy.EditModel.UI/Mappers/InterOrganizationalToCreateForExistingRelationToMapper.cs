namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class InterOrganizationalToCreateForExistingRelationToMapper(
IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper
) : IEnumerableMapper<EditModel.InterOrganizationalRelation.To.Complete.Resolved.ToCreate, CreateModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants>
{
    public IEnumerable<CreateModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants> Map(IEnumerable<EditModel.InterOrganizationalRelation.To.Complete.Resolved.ToCreate> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new CreateModel.InterOrganizationalRelation.ToCreate.ForExistingParticipants {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                OrganizationIdFrom = relation.OrganizationFrom.Id,
                OrganizationIdTo = relation.OrganizationTo.Id,
                InterOrganizationalRelationDetails = new CreateModel.InterOrganizationalRelationDetails {
                    GeographicalEntityId = relation.InterOrganizationalRelationDetails.GeographicalEntity?.Id,
                    InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationDetails.InterOrganizationalRelationType.Id,
                    DateRange = relation.RelationDetails.DateRange is null ? new DateTimeRange(null, null) : relation.RelationDetails.DateRange,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description,
                    MoneyInvolved = relation.InterOrganizationalRelationDetails.MoneyInvolved,
                    NumberOfChildrenInvolved = relation.InterOrganizationalRelationDetails.NumberOfChildrenInvolved,
                },
            };
        }
    }
}
