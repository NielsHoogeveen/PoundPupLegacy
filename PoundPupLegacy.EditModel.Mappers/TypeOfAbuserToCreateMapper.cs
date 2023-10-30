namespace PoundPupLegacy.EditModel.Mappers;

internal class TypeOfAbuserToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<TypeOfAbuser.ToCreate, DomainModel.TypeOfAbuser.ToCreate>
{
    public DomainModel.TypeOfAbuser.ToCreate Map(TypeOfAbuser.ToCreate source)
    {
        return new DomainModel.TypeOfAbuser.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
