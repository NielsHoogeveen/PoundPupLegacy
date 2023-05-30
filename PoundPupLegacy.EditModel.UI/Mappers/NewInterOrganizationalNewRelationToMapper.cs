using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewInterOrganizationalNewRelationToMapper(
    IMapper<EditModel.NodeDetails.NodeDetailsForCreate, CreateModel.NodeDetails.NodeDetailsForCreate> nodeDetailMapper
 ) : IEnumerableMapper<NewInterOrganizationalExistingRelationTo, CreateModel.InterOrganizationalRelation.InterOrganizationalRelationToCreateForNewOrganizationTo>
{
    public IEnumerable<CreateModel.InterOrganizationalRelation.InterOrganizationalRelationToCreateForNewOrganizationTo> Map(IEnumerable<NewInterOrganizationalExistingRelationTo> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new CreateModel.InterOrganizationalRelation.InterOrganizationalRelationToCreateForNewOrganizationTo {
                IdentificationForCreate = new Identification.IdentificationForCreate {
                    Id = null,
                },
                NodeDetailsForCreate = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                OrganizationIdFrom = relation.OrganizationFrom.Id,
                InterOrganizationalRelationDetails = new CreateModel.InterOrganizationalRelationDetails {
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
