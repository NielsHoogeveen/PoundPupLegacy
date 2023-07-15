namespace PoundPupLegacy.EditModel.Mappers;

internal class InterPersonalRelationsToToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper
    ) : IEnumerableMapper<InterPersonalRelation.To.Complete.Resolved.ToUpdate, CreateModel.InterPersonalRelation.ToUpdate>
{
    public IEnumerable<CreateModel.InterPersonalRelation.ToUpdate> Map(IEnumerable<InterPersonalRelation.To.Complete.Resolved.ToUpdate> source)
    {
        foreach (var relation in source) {
            if (relation.RelationDetails.HasBeenDeleted)
                continue;
            yield return new CreateModel.InterPersonalRelation.ToUpdate {
                Identification = new Identification.Certain {
                    Id = relation.NodeIdentification.NodeId
                },
                NodeDetails = nodeDetailsMapper.Map(relation.NodeDetailsForUpdate),
                InterPersonalRelationDetails = new CreateModel.InterPersonalRelationDetails {
                    InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id,
                    DateRange = relation.RelationDetails.DateRange ?? new DateTimeRange(null, null),
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description,
                },
                PersonIdFrom = relation.PersonFrom.Id,
                PersonIdTo = relation.PersonTo.Id,
            };
        }
    }
}
