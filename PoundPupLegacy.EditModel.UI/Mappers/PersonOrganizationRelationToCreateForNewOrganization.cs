namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class PersonOrganizationRelationToCreateForNewOrganization(
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailMapper
) : IEnumerableMapper<EditModel.PersonOrganizationRelation.ForOrganization.Complete.ToCreateForNewOrganization, CreateModel.PersonOrganizationRelation.ToCreate.ForNewOrganization>
{
    public IEnumerable<CreateModel.PersonOrganizationRelation.ToCreate.ForNewOrganization> Map(IEnumerable<PersonOrganizationRelation.ForOrganization.Complete.ToCreateForNewOrganization> source)
    {
        foreach(var element in source) {
            yield return new CreateModel.PersonOrganizationRelation.ToCreate.ForNewOrganization {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(element.NodeDetailsForCreate),
                PersonId = element.Person.Id,
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
