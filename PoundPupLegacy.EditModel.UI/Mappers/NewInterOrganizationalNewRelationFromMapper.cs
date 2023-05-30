namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewInterOrganizationalNewRelationFromMapper(
    IMapper<EditModel.NodeDetails.NodeDetailsForCreate, CreateModel.NodeDetails.NodeDetailsForCreate> nodeDetailMapper
 ) : IEnumerableMapper<NewInterOrganizationalExistingRelationFrom, CreateModel.InterOrganizationalRelation.ToCreateForNewOrganizationFrom>
{
    public IEnumerable<CreateModel.InterOrganizationalRelation.ToCreateForNewOrganizationFrom> Map(IEnumerable<NewInterOrganizationalExistingRelationFrom> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new CreateModel.InterOrganizationalRelation.ToCreateForNewOrganizationFrom {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                OrganizationIdTo = relation.OrganizationTo.Id,
                InterOrganizationalRelationDetails = new CreateModel.InterOrganizationalRelationDetails {
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
