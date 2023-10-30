namespace PoundPupLegacy.EditModel.Mappers;

internal class InterPersonalRelationTypeToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<InterPersonalRelationType.ToCreate, DomainModel.InterPersonalRelationType.ToCreate>
{
    public DomainModel.InterPersonalRelationType.ToCreate Map(InterPersonalRelationType.ToCreate source)
    {
        return new DomainModel.InterPersonalRelationType.ToCreate {
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
