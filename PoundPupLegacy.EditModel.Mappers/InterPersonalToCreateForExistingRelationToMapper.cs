namespace PoundPupLegacy.EditModel.Mappers;

internal class InterPersonalToCreateForExistingRelationToMapper(
IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper
) : IEnumerableMapper<InterPersonalRelation.To.Complete.Resolved.ToCreate, DomainModel.InterPersonalRelation.ToCreate.ForExistingParticipants>
{
    public IEnumerable<DomainModel.InterPersonalRelation.ToCreate.ForExistingParticipants> Map(IEnumerable<InterPersonalRelation.To.Complete.Resolved.ToCreate> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new DomainModel.InterPersonalRelation.ToCreate.ForExistingParticipants {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                PersonIdFrom = relation.PersonFrom.Id,
                PersonIdTo = relation.PersonTo.Id,
                InterPersonalRelationDetails = new DomainModel.InterPersonalRelationDetails {
                    InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id,
                    DateRange = relation.RelationDetails.DateRange is null ? new DateTimeRange(null, null) : relation.RelationDetails.DateRange,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description
                },
            };
        }
    }
}
