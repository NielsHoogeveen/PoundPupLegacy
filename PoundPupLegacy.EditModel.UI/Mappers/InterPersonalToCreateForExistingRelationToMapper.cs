﻿namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class InterPersonalToCreateForExistingRelationToMapper(
IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper
) : IEnumerableMapper<EditModel.InterPersonalRelation.To.Complete.Resolved.ToCreate, CreateModel.InterPersonalRelation.ToCreate.ForExistingParticipants>
{
    public IEnumerable<CreateModel.InterPersonalRelation.ToCreate.ForExistingParticipants> Map(IEnumerable<EditModel.InterPersonalRelation.To.Complete.Resolved.ToCreate> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new CreateModel.InterPersonalRelation.ToCreate.ForExistingParticipants {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                PersonIdFrom = relation.PersonFrom.Id,
                PersonIdTo = relation.PersonTo.Id,
                InterPersonalRelationDetails = new CreateModel.InterPersonalRelationDetails {
                    InterPersonalRelationTypeId = relation.InterPersonalRelationType.Id,
                    DateRange = relation.RelationDetails.DateRange is null ? new DateTimeRange(null, null) : relation.RelationDetails.DateRange,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description
                },
            };
        }
    }
}
