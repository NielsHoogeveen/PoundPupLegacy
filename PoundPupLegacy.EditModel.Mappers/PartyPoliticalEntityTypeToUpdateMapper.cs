namespace PoundPupLegacy.EditModel.Mappers;

internal class PartyPoliticalEntityRelationTypeToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<PartyPoliticalEntityRelationType.ToUpdate, DomainModel.PartyPoliticalEntityRelationType.ToUpdate>
{
    public DomainModel.PartyPoliticalEntityRelationType.ToUpdate Map(PartyPoliticalEntityRelationType.ToUpdate source)
    {
        return new DomainModel.PartyPoliticalEntityRelationType.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            PartyPoliticalEntityRelationTypeDetails = new DomainModel.PartyPoliticalEntityRelationTypeDetails {
                HasConcreteSubtype = false
            }
        };
    }
}
