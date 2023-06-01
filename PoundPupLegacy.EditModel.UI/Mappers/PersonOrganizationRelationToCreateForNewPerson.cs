namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class PersonOrganizationRelationToCreateForNewPerson(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper
) : IEnumerableMapper<EditModel.PersonOrganizationRelation.ForPerson.Complete.ToCreateForNewPerson, CreateModel.PersonOrganizationRelation.ToCreate.ForNewPerson>
{
    public IEnumerable<CreateModel.PersonOrganizationRelation.ToCreate.ForNewPerson> Map(IEnumerable<PersonOrganizationRelation.ForPerson.Complete.ToCreateForNewPerson> source)
    {
        foreach(var element in source) {
            yield return new CreateModel.PersonOrganizationRelation.ToCreate.ForNewPerson {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(element.NodeDetailsForCreate),
                OrganizationId = element.Organization.Id,
                PersonOrganizationRelationDetails = new CreateModel.PersonOrganizationRelationDetails {
                    DateRange = element.RelationDetails.DateRange is null
                        ? new DateTimeRange(null, null)
                        : element.RelationDetails.DateRange,
                    DocumentIdProof = element.RelationDetails.ProofDocument?.Id,
                    Description = element.RelationDetails.Description,
                    GeographicalEntityId = element.GeographicalEntity?.Id,
                    PersonOrganizationRelationTypeId = element.PersonOrganizationRelationType.Id,
                },
            };
        }
    }
}
