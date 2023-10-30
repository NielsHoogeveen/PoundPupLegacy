namespace PoundPupLegacy.EditModel.Mappers;

internal class DenominationToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<Denomination.ToUpdate, DomainModel.Denomination.ToUpdate>
{
    public DomainModel.Denomination.ToUpdate Map(Denomination.ToUpdate source)
    {
        return new DomainModel.Denomination.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
