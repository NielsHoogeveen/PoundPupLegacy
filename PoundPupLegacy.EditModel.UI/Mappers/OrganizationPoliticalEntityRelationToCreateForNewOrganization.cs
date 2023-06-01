using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class OrganizationPoliticalEntityRelationToCreateForNewOrganization(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper
    ) : IEnumerableMapper<EditModel.OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization, CreateModel.PartyPoliticalEntityRelation.ToCreate.ForNewParty>
{
    public IEnumerable<PartyPoliticalEntityRelation.ToCreate.ForNewParty> Map(IEnumerable<OrganizationPoliticalEntityRelation.Complete.ToCreateForNewOrganization> source)
    {
        foreach(var element in source) {
            yield return new PartyPoliticalEntityRelation.ToCreate.ForNewParty {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(element.NodeDetailsForCreate),
                PartyPoliticalEntityRelationDetails = new PartyPoliticalEntityRelationDetails {
                    DateRange = element.RelationDetails.DateRange is null
                        ? new DateTimeRange(null, null)
                        : element.RelationDetails.DateRange,
                    DocumentIdProof = element.RelationDetails.ProofDocument?.Id,
                    PartyPoliticalEntityRelationTypeId = element.OrganizationPoliticalEntityRelationType.Id,
                    PoliticalEntityId = element.PoliticalEntity.Id
                }
            };
        }
    }
}
