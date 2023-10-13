using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class OrganizationPoliticalEntityRelationToCreateForExistingOrganization(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper
    ) : IEnumerableMapper<OrganizationPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingOrganization, PartyPoliticalEntityRelation.ToCreate.ForExistingParty>
{
    public IEnumerable<PartyPoliticalEntityRelation.ToCreate.ForExistingParty> Map(IEnumerable<OrganizationPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingOrganization> source)
    {
        foreach (var element in source) {
            element.NodeDetails.Title = $"{element.OrganizationName} {element.PartyPoliticalEntityRelationType.Name.ToLower()} {element.PoliticalEntityName}";
            yield return new PartyPoliticalEntityRelation.ToCreate.ForExistingParty {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(element.NodeDetailsForCreate),
                PartyId = element.Organization.Id,
                PartyPoliticalEntityRelationDetails = new PartyPoliticalEntityRelationDetails {
                    DateRange = element.RelationDetails.DateRange is null
                        ? new DateTimeRange(null, null)
                        : element.RelationDetails.DateRange,
                    DocumentIdProof = element.RelationDetails.ProofDocument?.Id,
                    PartyPoliticalEntityRelationTypeId = element.PartyPoliticalEntityRelationType.Id,
                    PoliticalEntityId = element.PoliticalEntity.Id,
                    Description = element.RelationDetails.Description,
                }
            };
        }
    }
}
