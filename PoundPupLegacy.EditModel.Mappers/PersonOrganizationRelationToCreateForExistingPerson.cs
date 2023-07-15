namespace PoundPupLegacy.EditModel.Mappers;

internal class PersonOrganizationRelationToCreateForExistingPerson(
    IMapper<NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper
    ) : IEnumerableMapper<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate, CreateModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants>
{
    public IEnumerable<CreateModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants> Map(IEnumerable<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate> source)
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
