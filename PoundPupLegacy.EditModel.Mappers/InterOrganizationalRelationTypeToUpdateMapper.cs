namespace PoundPupLegacy.EditModel.Mappers;

internal class InterOrganizationalRelationTypeToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<InterOrganizationalRelationType.ToUpdate, DomainModel.InterOrganizationalRelationType.ToUpdate>
{
    public DomainModel.InterOrganizationalRelationType.ToUpdate Map(InterOrganizationalRelationType.ToUpdate source)
    {
        return new DomainModel.InterOrganizationalRelationType.ToUpdate {
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
