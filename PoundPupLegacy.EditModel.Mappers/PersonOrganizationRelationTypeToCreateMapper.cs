namespace PoundPupLegacy.EditModel.Mappers;

internal class PersonOrganizationRelationTypeToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<PersonOrganizationRelationType.ToCreate, DomainModel.PersonOrganizationRelationType.ToCreate>
{
    public DomainModel.PersonOrganizationRelationType.ToCreate Map(PersonOrganizationRelationType.ToCreate source)
    {
        return new DomainModel.PersonOrganizationRelationType.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            PersonOrganizationRelationTypeDetails = new DomainModel.PersonOrganizationRelationTypeDetails {
                HasConcreteSubtype = false
            }
        };
    }
}
