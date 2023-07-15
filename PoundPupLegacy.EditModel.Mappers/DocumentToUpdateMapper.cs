using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Mappers;

internal class DocumentToUpdateMapper(
    ITextService textService,
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailsMapper
    ) : IMapper<Document.ToUpdate, DomainModel.Document.ToUpdate>
{
    public DomainModel.Document.ToUpdate Map(Document.ToUpdate source)
    {
        return new DomainModel.Document.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId
            },
            NodeDetails = nodeDetailsMapper.Map(source.NodeDetailsForUpdate),
            SimpleTextNodeDetails = new DomainModel.SimpleTextNodeDetails {
                Text = textService.FormatText(source.SimpleTextNodeDetails.Text),
                Teaser = textService.FormatTeaser(source.SimpleTextNodeDetails.Text)
            },
            DocumentDetails = new DomainModel.DocumentDetails {
                DocumentTypeId = source.DocumentDetails.DocumentTypeId,
                Published = source.DocumentDetails.Published,
                SourceUrl = source.DocumentDetails.SourceUrl,
            },
        };
    }
}
