using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class OrganizationPoliticalEntityRelationToUpdate(
        IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailMapper
) : IEnumerableMapper<EditModel.OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate, CreateModel.PartyPoliticalEntityRelation.ToUpdate>
{
    public IEnumerable<PartyPoliticalEntityRelation.ToUpdate> Map(IEnumerable<OrganizationPoliticalEntityRelation.Complete.Resolved.ToUpdate> source)
    {
        foreach(var element in source) {
            yield return new PartyPoliticalEntityRelation.ToUpdate {
                Identification = new Identification.Certain {
                    Id = element.NodeIdentification.NodeId,
                },
                NodeDetails = nodeDetailMapper.Map(element.NodeDetailsForUpdate),
                PartyId = element.Organization.Id,
                PartyPoliticalEntityRelationDetails = new PartyPoliticalEntityRelationDetails {
                    DateRange = element.RelationDetails.DateRange is null
                        ? new DateTimeRange(null, null)
                        : element.RelationDetails.DateRange,
                    DocumentIdProof = element.RelationDetails.ProofDocument?.Id,
                    PartyPoliticalEntityRelationTypeId = element.OrganizationPoliticalEntityRelationType.Id,
                    PoliticalEntityId = element.PoliticalEntity.Id,
                }
            };
        }
    }
}
