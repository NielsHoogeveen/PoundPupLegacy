namespace PoundPupLegacy.EditModel.Mappers;

internal class PersonOrganizationRelationToCreateForNewOrganization(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper
) : IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.ToCreateForNewOrganization, DomainModel.PersonOrganizationRelation.ToCreate.ForNewOrganization>
{
    public IEnumerable<DomainModel.PersonOrganizationRelation.ToCreate.ForNewOrganization> Map(IEnumerable<PersonOrganizationRelation.ForOrganization.Complete.ToCreateForNewOrganization> source)
    {
        foreach (var element in source) {
            element.NodeDetails.Title = $"{element.PersonName} {element.PersonOrganizationRelationType.Name.ToLower()} {element.OrganizationName}";
            yield return new DomainModel.PersonOrganizationRelation.ToCreate.ForNewOrganization {
                Identification = new Identification.Possible {
                    Id = null,
                },
                NodeDetails = nodeDetailMapper.Map(element.NodeDetailsForCreate),
                PersonId = element.Person.Id,
                PersonOrganizationRelationDetails = new DomainModel.PersonOrganizationRelationDetails {
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
