namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewInterOrganizationalNewRelationFromMapper(
    IMapper<EditModel.NodeDetails.NodeDetailsForCreate, CreateModel.NodeDetails.NodeDetailsForCreate> nodeDetailMapper
 ) : IEnumerableMapper<NewInterOrganizationalExistingRelationFrom, CreateModel.InterOrganizationalRelation.InterOrganizationalRelationToCreateForNewOrganizationFrom>
{
    public IEnumerable<CreateModel.InterOrganizationalRelation.InterOrganizationalRelationToCreateForNewOrganizationFrom> Map(IEnumerable<NewInterOrganizationalExistingRelationFrom> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new CreateModel.InterOrganizationalRelation.InterOrganizationalRelationToCreateForNewOrganizationFrom {
                IdentificationForCreate = new Identification.IdentificationForCreate {
                    Id = null,
                },
                NodeDetailsForCreate = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
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
