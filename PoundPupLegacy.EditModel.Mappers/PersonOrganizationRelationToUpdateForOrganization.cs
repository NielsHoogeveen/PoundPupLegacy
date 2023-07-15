namespace PoundPupLegacy.EditModel.Mappers;

internal class PersonOrganizationRelationToUpdateForOrganization(
        IMapper<NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailMapper
) : IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate, CreateModel.PersonOrganizationRelation.ToUpdate>
{
    public IEnumerable<CreateModel.PersonOrganizationRelation.ToUpdate> Map(IEnumerable<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate> source)
    {
        foreach (var element in source) {
            yield return new CreateModel.PersonOrganizationRelation.ToUpdate {
                Identification = new Identification.Certain {
                    Id = element.NodeIdentification.NodeId
                },
                NodeDetails = nodeDetailMapper.Map(element.NodeDetailsForUpdate),
                OrganizationId = element.Organization.Id,
                PersonId = element.Person.Id,
                PersonOrganizationRelationDetails = new CreateModel.PersonOrganizationRelationDetails {
                    DateRange = element.RelationDetails.DateRange is null
                        ? new DateTimeRange(null, null)
                        : element.RelationDetails.DateRange,
                    DocumentIdProof = element.RelationDetails.ProofDocument?.Id,
                    Description = element.RelationDetails.Description,
                    GeographicalEntityId = element.GeographicalEntity?.Id,
                    PersonOrganizationRelationTypeId = element.PersonOrganizationRelationType.Id,
                }
            };
        }
    }
}
