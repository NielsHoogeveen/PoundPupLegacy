namespace PoundPupLegacy.EditModel.Mappers;

internal class PersonOrganizationRelationToUpdateForOrganization(
        IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper
) : IEnumerableMapper<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate, DomainModel.PersonOrganizationRelation.ToUpdate>
{
    public IEnumerable<DomainModel.PersonOrganizationRelation.ToUpdate> Map(IEnumerable<PersonOrganizationRelation.ForOrganization.Complete.Resolved.ToUpdate> source)
    {
        foreach (var element in source) {
            element.NodeDetails.Title = $"{element.PersonName} {element.PersonOrganizationRelationType.Name.ToLower()} {element.OrganizationName}";
            yield return new DomainModel.PersonOrganizationRelation.ToUpdate {
                Identification = new Identification.Certain {
                    Id = element.NodeIdentification.NodeId
                },
                NodeDetails = nodeDetailMapper.Map(element.NodeDetailsForUpdate),
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
