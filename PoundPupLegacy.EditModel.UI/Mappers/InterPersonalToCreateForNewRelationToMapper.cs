namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class InterPersonalToCreateForNewRelationToMapper(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper
 ) : IEnumerableMapper<EditModel.InterPersonalRelation.To.Complete.ToCreateForNewPerson, CreateModel.InterPersonalRelation.ToCreate.ForNewPersonTo>
{
    public IEnumerable<CreateModel.InterPersonalRelation.ToCreate.ForNewPersonTo> Map(IEnumerable<EditModel.InterPersonalRelation.To.Complete.ToCreateForNewPerson> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new CreateModel.InterPersonalRelation.ToCreate.ForNewPersonTo {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                PersonIdFrom = relation.PersonFrom.Id,
                InterPersonalRelationDetails = new CreateModel.InterPersonalRelationDetails {
                    DateRange = relation.RelationDetails.DateRange is null ? new DateTimeRange(null, null) : relation.RelationDetails.DateRange,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description,
                    InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id,
                },
            };
        }
    }
}
