using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.EditModel.Mappers;

internal class OrganizationTypeToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<OrganizationType.ToUpdate, DomainModel.OrganizationType.ToUpdate>
{
    public DomainModel.OrganizationType.ToUpdate Map(OrganizationType.ToUpdate source)
    {
        return new DomainModel.OrganizationType.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            OrganizationTypeDetails = new OrganizationTypeDetails {
                HasConcreteSubtype = false
            }
        };
    }
}
