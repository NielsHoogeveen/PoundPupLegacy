namespace PoundPupLegacy.EditModel.Mappers;

internal class ChildPlacementTypeToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<ChildPlacementType.ToCreate, DomainModel.ChildPlacementType.ToCreate>
{
    public DomainModel.ChildPlacementType.ToCreate Map(ChildPlacementType.ToCreate source)
    {
        return new DomainModel.ChildPlacementType.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
