namespace PoundPupLegacy.EditModel.Mappers;

internal class InterOrganizationalRelationTypeToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<InterOrganizationalRelationType.ToCreate, DomainModel.InterOrganizationalRelationType.ToCreate>
{
    public DomainModel.InterOrganizationalRelationType.ToCreate Map(InterOrganizationalRelationType.ToCreate source)
    {
        return new DomainModel.InterOrganizationalRelationType.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            EndoRelationTypeDetails = new DomainModel.EndoRelationTypeDetails { 
                IsSymmetric = source.IsSymmetric
            }
        };
    }
}
