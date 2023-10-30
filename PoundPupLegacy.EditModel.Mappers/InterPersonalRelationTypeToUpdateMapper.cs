namespace PoundPupLegacy.EditModel.Mappers;

internal class InterPersonalRelationTypeToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<InterPersonalRelationType.ToUpdate, DomainModel.InterPersonalRelationType.ToUpdate>
{
    public DomainModel.InterPersonalRelationType.ToUpdate Map(InterPersonalRelationType.ToUpdate source)
    {
        return new DomainModel.InterPersonalRelationType.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            EndoRelationTypeDetails = new DomainModel.EndoRelationTypeDetails { 
                IsSymmetric = source.IsSymmetric
            }
        };
    }
}
