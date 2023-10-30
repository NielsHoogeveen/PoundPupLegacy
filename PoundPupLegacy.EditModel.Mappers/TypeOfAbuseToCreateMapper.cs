namespace PoundPupLegacy.EditModel.Mappers;

internal class TypeOfAbuseToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<TypeOfAbuse.ToCreate, DomainModel.TypeOfAbuse.ToCreate>
{
    public DomainModel.TypeOfAbuse.ToCreate Map(TypeOfAbuse.ToCreate source)
    {
        return new DomainModel.TypeOfAbuse.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
