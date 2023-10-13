﻿using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class PersonPoliticalEntityRelationToCreateForNewPerson(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper
    ) : IEnumerableMapper<PersonPoliticalEntityRelation.Complete.ToCreateForNewPerson, PartyPoliticalEntityRelation.ToCreate.ForNewParty>
{
    public IEnumerable<PartyPoliticalEntityRelation.ToCreate.ForNewParty> Map(IEnumerable<PersonPoliticalEntityRelation.Complete.ToCreateForNewPerson> source)
    {
        foreach (var element in source) {
            element.NodeDetails.Title = $"{element.PersonName} {element.PartyPoliticalEntityRelationType.Name.ToLower()} {element.PoliticalEntityName}";
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
                    PartyPoliticalEntityRelationTypeId = element.PartyPoliticalEntityRelationType.Id,
                    PoliticalEntityId = element.PoliticalEntity.Id,
                    Description = element.RelationDetails.Description,
                }
            };
        }
    }
}
