namespace PoundPupLegacy.EditModel.Mappers;

internal class InterPersonalRelationsFromToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper
) : IEnumerableMapper<InterPersonalRelation.From.Complete.Resolved.ToUpdate, DomainModel.InterPersonalRelation.ToUpdate>
{
    public IEnumerable<DomainModel.InterPersonalRelation.ToUpdate> Map(IEnumerable<InterPersonalRelation.From.Complete.Resolved.ToUpdate> source)
    {
        foreach (var relation in source) {
            if (relation.RelationDetails.HasBeenDeleted)
                continue;
            relation.NodeDetails.Title = $"{relation.PersonFromName} {relation.InterPersonalRelationType.Name.ToLower()} {relation.PersonToName}";                  
            yield return new DomainModel.InterPersonalRelation.ToUpdate {
                Identification = new Identification.Certain {
                    Id = relation.NodeIdentification.NodeId,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForUpdate),
                PersonIdFrom = relation.PersonFrom.Id,
                PersonIdTo = relation.PersonTo.Id,
                InterPersonalRelationDetails = new DomainModel.InterPersonalRelationDetails {
                    InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id,
                    DateRange = relation.RelationDetails.DateRange ?? new DateTimeRange(null, null),
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description,
                }
            };
        }
    }
}
