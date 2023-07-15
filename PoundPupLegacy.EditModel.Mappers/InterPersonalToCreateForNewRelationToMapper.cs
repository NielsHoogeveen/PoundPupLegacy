namespace PoundPupLegacy.EditModel.Mappers;

internal class InterPersonalToCreateForNewRelationToMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper
 ) : IEnumerableMapper<InterPersonalRelation.To.Complete.ToCreateForNewPerson, DomainModel.InterPersonalRelation.ToCreate.ForNewPersonTo>
{
    public IEnumerable<DomainModel.InterPersonalRelation.ToCreate.ForNewPersonTo> Map(IEnumerable<InterPersonalRelation.To.Complete.ToCreateForNewPerson> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new DomainModel.InterPersonalRelation.ToCreate.ForNewPersonTo {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                PersonIdFrom = relation.PersonFrom.Id,
                InterPersonalRelationDetails = new DomainModel.InterPersonalRelationDetails {
                    DateRange = relation.RelationDetails.DateRange is null ? new DateTimeRange(null, null) : relation.RelationDetails.DateRange,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description,
                    InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id,
                },
            };
        }
    }
}
