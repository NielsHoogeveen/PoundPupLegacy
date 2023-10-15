namespace PoundPupLegacy.EditModel.Mappers;

internal class OrganizationTypeToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<OrganizationType.ToCreate, DomainModel.OrganizationType.ToCreate>
{
    public DomainModel.OrganizationType.ToCreate Map(OrganizationType.ToCreate source)
    {
        return new DomainModel.OrganizationType.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            OrganizationTypeDetails = new DomainModel.OrganizationTypeDetails { 
                HasConcreteSubtype = false
            }
        };
    }
}
