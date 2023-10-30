namespace PoundPupLegacy.EditModel.Mappers;

internal class ProfessionToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<Profession.ToUpdate, DomainModel.Profession.ToUpdate>
{
    public DomainModel.Profession.ToUpdate Map(Profession.ToUpdate source)
    {
        return new DomainModel.Profession.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            ProfessionDetails = new DomainModel.ProfessionDetails { 
                HasConcreteSubtype = false 
            }
        };
    }
}
