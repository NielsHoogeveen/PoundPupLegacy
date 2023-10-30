namespace PoundPupLegacy.EditModel.Mappers;

internal class DenominationToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<Denomination.ToCreate, DomainModel.Denomination.ToCreate>
{
    public DomainModel.Denomination.ToCreate Map(Denomination.ToCreate source)
    {
        return new DomainModel.Denomination.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
