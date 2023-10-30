namespace PoundPupLegacy.EditModel.Mappers;

internal class BasicNameableToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<BasicNameable.ToCreate, DomainModel.BasicNameable.ToCreate>
{
    public DomainModel.BasicNameable.ToCreate Map(BasicNameable.ToCreate source)
    {
        return new DomainModel.BasicNameable.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
