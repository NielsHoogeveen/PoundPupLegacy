namespace PoundPupLegacy.EditModel.Mappers;

internal class TypeOfAbuserToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<TypeOfAbuser.ToUpdate, DomainModel.TypeOfAbuser.ToUpdate>
{
    public DomainModel.TypeOfAbuser.ToUpdate Map(TypeOfAbuser.ToUpdate source)
    {
        return new DomainModel.TypeOfAbuser.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
