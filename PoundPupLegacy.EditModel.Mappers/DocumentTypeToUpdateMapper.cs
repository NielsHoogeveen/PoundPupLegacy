namespace PoundPupLegacy.EditModel.Mappers;

internal class DocumentTypeToUpdateMapper(
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForUpdate> nameableDetailMapper
) : IMapper<DocumentType.ToUpdate, DomainModel.DocumentType.ToUpdate>
{
    public DomainModel.DocumentType.ToUpdate Map(DocumentType.ToUpdate source)
    {
        return new DomainModel.DocumentType.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForUpdate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
