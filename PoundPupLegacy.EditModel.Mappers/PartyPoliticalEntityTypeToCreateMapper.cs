namespace PoundPupLegacy.EditModel.Mappers;

internal class PartyPoliticalEntityRelationTypeToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<PartyPoliticalEntityRelationType.ToCreate, DomainModel.PartyPoliticalEntityRelationType.ToCreate>
{
    public DomainModel.PartyPoliticalEntityRelationType.ToCreate Map(PartyPoliticalEntityRelationType.ToCreate source)
    {
        return new DomainModel.PartyPoliticalEntityRelationType.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            PartyPoliticalEntityRelationTypeDetails = new DomainModel.PartyPoliticalEntityRelationTypeDetails { 
                HasConcreteSubtype = false
            }
        };
    }
}
