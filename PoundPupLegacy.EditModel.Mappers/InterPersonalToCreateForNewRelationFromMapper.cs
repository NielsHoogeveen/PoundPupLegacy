namespace PoundPupLegacy.EditModel.Mappers;

internal class InterPersonalToCreateForNewRelationFromMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper
 ) : IEnumerableMapper<InterPersonalRelation.From.Complete.ToCreateForNewPerson, DomainModel.InterPersonalRelation.ToCreate.ForNewPersonFrom>
{
    public IEnumerable<DomainModel.InterPersonalRelation.ToCreate.ForNewPersonFrom> Map(IEnumerable<InterPersonalRelation.From.Complete.ToCreateForNewPerson> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            relation.NodeDetails.Title = $"{relation.PersonFromName} {relation.InterPersonalRelationType.Name.ToLower()} {relation.PersonToName}";            relation.NodeDetails.Title = $"{relation.PersonFromName} {relation.InterPersonalRelationType.Name.ToLower()} {relation.PersonToName}";
            yield return new DomainModel.InterPersonalRelation.ToCreate.ForNewPersonFrom {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                PersonIdTo = relation.PersonTo.Id,
                InterPersonalRelationDetails = new DomainModel.InterPersonalRelationDetails {
                    InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id,
                    DateRange = relation.RelationDetails.DateRange is null ? new DateTimeRange(null, null) : relation.RelationDetails.DateRange,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description,
                }
            };
        }
    }
}
