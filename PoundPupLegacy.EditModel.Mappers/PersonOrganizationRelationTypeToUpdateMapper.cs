namespace PoundPupLegacy.EditModel.Mappers;

internal class PersonOrganizationRelationTypeToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<PersonOrganizationRelationType.ToUpdate, DomainModel.PersonOrganizationRelationType.ToUpdate>
{
    public DomainModel.PersonOrganizationRelationType.ToUpdate Map(PersonOrganizationRelationType.ToUpdate source)
    {
        return new DomainModel.PersonOrganizationRelationType.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
            PersonOrganizationRelationTypeDetails = new DomainModel.PersonOrganizationRelationTypeDetails {
                HasConcreteSubtype = false
            }
        };
    }
}
