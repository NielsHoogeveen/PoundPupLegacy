namespace PoundPupLegacy.EditModel.Mappers;

internal class HagueStatusToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<HagueStatus.ToCreate, DomainModel.HagueStatus.ToCreate>
{
    public DomainModel.HagueStatus.ToCreate Map(HagueStatus.ToCreate source)
    {
        return new DomainModel.HagueStatus.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
