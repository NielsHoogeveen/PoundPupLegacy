namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class InterPersonalToCreateForNewRelationFromMapper(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper
 ) : IEnumerableMapper<InterPersonalRelation.From.Complete.ToCreateForNewPerson, CreateModel.InterPersonalRelation.ToCreate.ForNewPersonFrom>
{
    public IEnumerable<CreateModel.InterPersonalRelation.ToCreate.ForNewPersonFrom> Map(IEnumerable<InterPersonalRelation.From.Complete.ToCreateForNewPerson> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new CreateModel.InterPersonalRelation.ToCreate.ForNewPersonFrom {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                PersonIdTo = relation.PersonTo.Id,
                InterPersonalRelationDetails = new CreateModel.InterPersonalRelationDetails {
                    InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id,
                    DateRange = relation.RelationDetails.DateRange is null ? new DateTimeRange(null, null) : relation.RelationDetails.DateRange,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description,
                }
            };
        }
    }
}
