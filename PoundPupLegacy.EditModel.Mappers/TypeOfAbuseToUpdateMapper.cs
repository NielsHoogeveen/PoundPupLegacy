namespace PoundPupLegacy.EditModel.Mappers;

internal class TypeOfAbuseToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<TypeOfAbuse.ToUpdate, DomainModel.TypeOfAbuse.ToUpdate>
{
    public DomainModel.TypeOfAbuse.ToUpdate Map(TypeOfAbuse.ToUpdate source)
    {
        return new DomainModel.TypeOfAbuse.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
