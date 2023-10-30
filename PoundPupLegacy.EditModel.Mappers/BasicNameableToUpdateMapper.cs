namespace PoundPupLegacy.EditModel.Mappers;

internal class BasicNameableToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<BasicNameable.ToUpdate, DomainModel.BasicNameable.ToUpdate>
{
    public DomainModel.BasicNameable.ToUpdate Map(BasicNameable.ToUpdate source)
    {
        return new DomainModel.BasicNameable.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
