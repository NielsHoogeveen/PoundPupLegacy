namespace PoundPupLegacy.EditModel.Mappers;

internal class PersonOrganizationRelationToCreateForExistingPerson(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper
    ) : IEnumerableMapper<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate, DomainModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants>
{
    public IEnumerable<DomainModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants> Map(IEnumerable<PersonOrganizationRelation.ForPerson.Complete.Resolved.ToCreate> source)
    {
        foreach (var element in source) {
            element.NodeDetails.Title = $"{element.PersonName} {element.PersonOrganizationRelationType.Name.ToLower()} {element.OrganizationName}";
            yield return new DomainModel.PersonOrganizationRelation.ToCreate.ForExistingParticipants {
                Identification = new Identification.Possible {
                    Id = null
                },
                NodeDetails = nodeDetailMapper.Map(element.NodeDetailsForCreate),
                OrganizationId = element.Organization.Id,
                PersonId = element.Person.Id,
                PersonOrganizationRelationDetails = new DomainModel.PersonOrganizationRelationDetails {
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
