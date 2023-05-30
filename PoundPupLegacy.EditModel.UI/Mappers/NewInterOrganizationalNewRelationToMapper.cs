﻿namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NewInterOrganizationalNewRelationToMapper(
    IMapper<EditModel.NodeDetails.NodeDetailsForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper
 ) : IEnumerableMapper<EditModel.InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization, CreateModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationTo>
{
    public IEnumerable<CreateModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationTo> Map(IEnumerable<EditModel.InterOrganizationalRelation.To.Complete.ToCreateForNewOrganization> source)
    {
        foreach (var relation in source) {
            var now = DateTime.Now;
            yield return new CreateModel.InterOrganizationalRelation.ToCreate.ForNewOrganizationTo {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(relation.NodeDetailsForCreate),
                OrganizationIdFrom = relation.OrganizationFrom.Id,
                InterOrganizationalRelationDetails = new CreateModel.InterOrganizationalRelationDetails {
                    GeographicalEntityId = relation.InterOrganizationalRelationDetails.GeographicalEntity?.Id,
                    DateRange = relation.RelationDetails.DateRange is null ? new DateTimeRange(null, null) : relation.RelationDetails.DateRange,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    Description = relation.RelationDetails.Description,
                    MoneyInvolved = relation.InterOrganizationalRelationDetails.MoneyInvolved,
                    NumberOfChildrenInvolved = relation.InterOrganizationalRelationDetails.NumberOfChildrenInvolved,
                    InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationDetails.InterOrganizationalRelationType.Id,
                },
            };
        }
    }
}
