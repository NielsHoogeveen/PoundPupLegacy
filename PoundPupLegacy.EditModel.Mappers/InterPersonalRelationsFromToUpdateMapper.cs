namespace PoundPupLegacy.EditModel.Mappers;

internal class InterPersonalRelationsFromToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailMapper
) : IEnumerableMapper<InterPersonalRelation.From.Complete.Resolved.ToUpdate, CreateModel.InterPersonalRelation.ToUpdate>
{
    public IEnumerable<CreateModel.InterPersonalRelation.ToUpdate> Map(IEnumerable<InterPersonalRelation.From.Complete.Resolved.ToUpdate> source)
    {
        foreach (var relation in source) {
            if (relation.RelationDetails.HasBeenDeleted)
                continue;
            yield return new CreateModel.InterPersonalRelation.ToUpdate {
                Identification = new Identification.Certain {
                    Id = relation.NodeIdentification.NodeId,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForUpdate),
                PersonIdFrom = relation.PersonFrom.Id,
                PersonIdTo = relation.PersonTo.Id,
                InterPersonalRelationDetails = new CreateModel.InterPersonalRelationDetails {
                    InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id,
                    DateRange = relation.RelationDetails.DateRange ?? new DateTimeRange(null, null),
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description,
                }
            };
        }
    }
}
