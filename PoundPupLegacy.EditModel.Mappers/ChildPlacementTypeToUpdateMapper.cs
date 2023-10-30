namespace PoundPupLegacy.EditModel.Mappers;

internal class ChildPlacementTypeToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<ChildPlacementType.ToUpdate, DomainModel.ChildPlacementType.ToUpdate>
{
    public DomainModel.ChildPlacementType.ToUpdate Map(ChildPlacementType.ToUpdate source)
    {
        return new DomainModel.ChildPlacementType.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
