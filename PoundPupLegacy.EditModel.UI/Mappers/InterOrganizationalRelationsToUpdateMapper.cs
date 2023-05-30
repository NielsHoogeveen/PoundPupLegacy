using PoundPupLegacy.CreateModel;
using PoundPupLegacy.CreateModel.Deleters;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class InterOrganizationalRelationsToUpdateMapper(
    IMapper<EditModel.NodeDetails.NodeDetailsForUpdate, CreateModel.NodeDetails.NodeDetailsForUpdate> nodeDetailsMapper
    ) : IEnumerableMapper<ExistingInterOrganizationalRelationTo, CreateModel.InterOrganizationalRelation.InterOrganizationalRelationToUpdate>
{
    public IEnumerable<CreateModel.InterOrganizationalRelation.InterOrganizationalRelationToUpdate> Map(IEnumerable<ExistingInterOrganizationalRelationTo> source)
    {
        foreach(var relation in source) {
            if(relation.RelationDetails.HasBeenDeleted)
                continue;
            yield return new CreateModel.InterOrganizationalRelation.InterOrganizationalRelationToUpdate {
                IdentificationForUpdate = new Identification.IdentificationForUpdate {
                    Id = relation.NodeIdentification.NodeId
                },
                NodeDetailsForUpdate = nodeDetailsMapper.Map(relation.NodeDetailsForUpdate),
                InterOrganizationalRelationDetails = new CreateModel.InterOrganizationalRelationDetails {
                    InterOrganizationalRelationTypeId = relation.InterOrganizationalRelationDetails.InterOrganizationalRelationType.Id,
                    DateRange = relation.RelationDetails.DateRange ?? new DateTimeRange(null, null),
                    GeographicalEntityId = relation.InterOrganizationalRelationDetails.GeographicalEntity?.Id,
                    DocumentIdProof = relation.RelationDetails.ProofDocument?.Id,
                    MoneyInvolved = relation.InterOrganizationalRelationDetails.MoneyInvolved,
                    NumberOfChildrenInvolved = relation.InterOrganizationalRelationDetails.NumberOfChildrenInvolved,
                    Description = relation.RelationDetails.Description,
                },                    
                OrganizationIdFrom = relation.OrganizationFrom.Id,
                OrganizationIdTo = relation.OrganizationTo.Id,
            };
        }
    }
}
