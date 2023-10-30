namespace PoundPupLegacy.EditModel.Mappers;

internal class HagueStatusToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<HagueStatus.ToUpdate, DomainModel.HagueStatus.ToUpdate>
{
    public DomainModel.HagueStatus.ToUpdate Map(HagueStatus.ToUpdate source)
    {
        return new DomainModel.HagueStatus.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
