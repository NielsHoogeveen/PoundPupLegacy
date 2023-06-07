using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class PersonPoliticalEntityRelationToCreateForExistingPerson(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper
    ) : IEnumerableMapper<EditModel.PersonPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingPerson, CreateModel.PartyPoliticalEntityRelation.ToCreate.ForExistingParty>
{
    public IEnumerable<PartyPoliticalEntityRelation.ToCreate.ForExistingParty> Map(IEnumerable<PersonPoliticalEntityRelation.Complete.Resolved.ToCreateForExistingPerson> source)
    {
        foreach(var element in source) {
            yield return new PartyPoliticalEntityRelation.ToCreate.ForExistingParty {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(element.NodeDetailsForCreate),
                PartyId = element.Person.Id,
                PartyPoliticalEntityRelationDetails = new PartyPoliticalEntityRelationDetails {
                    DateRange = element.RelationDetails.DateRange is null
                        ? new DateTimeRange(null, null)
                        : element.RelationDetails.DateRange,
                    DocumentIdProof = element.RelationDetails.ProofDocument?.Id,
                    PartyPoliticalEntityRelationTypeId = element.PartyPoliticalEntityRelationType.Id,
                    PoliticalEntityId = element.PoliticalEntity.Id,
                }
            };
        }
    }
}
