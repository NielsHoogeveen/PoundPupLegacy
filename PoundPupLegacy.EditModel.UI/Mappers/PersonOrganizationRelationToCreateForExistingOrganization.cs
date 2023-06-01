namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class PersonOrganizationRelationToCreateForExistingOrganization(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper
    ) : IEnumerableMapper<EditModel.PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToCreate, CreateModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants>
{
    public IEnumerable<CreateModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants> Map(IEnumerable<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToCreate> source)
    {
        foreach (var element in source) {
            yield return new CreateModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = nodeDetailMapper.Map(element.NodeDetailsForCreate),
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
