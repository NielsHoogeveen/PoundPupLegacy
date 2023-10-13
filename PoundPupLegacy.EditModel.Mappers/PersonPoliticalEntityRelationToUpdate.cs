﻿using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class PersonPoliticalEntityRelationToUpdate(
        IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper
) : IEnumerableMapper<PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate, PartyPoliticalEntityRelation.ToUpdate>
{
    public IEnumerable<PartyPoliticalEntityRelation.ToUpdate> Map(IEnumerable<PersonPoliticalEntityRelation.Complete.Resolved.ToUpdate> source)
    {
        foreach (var element in source) {
            element.NodeDetails.Title = $"{element.PersonName} {element.PartyPoliticalEntityRelationType.Name.ToLower()} {element.PoliticalEntityName}";
            yield return new PartyPoliticalEntityRelation.ToUpdate {
                Identification = new Identification.Certain {
                    Id = element.NodeIdentification.NodeId,
                },
                NodeDetails = nodeDetailMapper.Map(element.NodeDetailsForUpdate),
                PartyId = element.Party.Id,
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
