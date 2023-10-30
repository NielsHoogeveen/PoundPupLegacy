namespace PoundPupLegacy.EditModel.Mappers;

internal class DocumentTypeToCreateMapper(
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailMapper,
    IMapper<NameableDetails, DomainModel.NameableDetails.ForCreate> nameableDetailMapper
) : IMapper<DocumentType.ToCreate, DomainModel.DocumentType.ToCreate>
{
    public DomainModel.DocumentType.ToCreate Map(DocumentType.ToCreate source)
    {
        return new DomainModel.DocumentType.ToCreate {
            Identification = new Identification.Possible {
                Id = null,
            },
            NodeDetails = nodeDetailMapper.Map(source.NodeDetailsForCreate),
            NameableDetails = nameableDetailMapper.Map(source.NameableDetails),
        };
    }
}
