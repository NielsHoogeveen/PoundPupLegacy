namespace PoundPupLegacy.EditModel.Mappers;

internal class ProfessionToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<Profession.ToCreate, DomainModel.Profession.ToCreate>
{
    public DomainModel.Profession.ToCreate Map(Profession.ToCreate source)
    {
        return new DomainModel.Profession.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            ProfessionDetails = new DomainModel.ProfessionDetails {
                HasConcreteSubtype = false 
            }
        };
    }
}
