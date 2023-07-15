using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Mappers;

internal class DocumentToUpdateMapper(
    ITextService textService,
    IMapper<NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper
    ) : IMapper<Document.ToUpdate, CreateModel.Document.ToUpdate>
{
    public CreateModel.Document.ToUpdate Map(Document.ToUpdate source)
    {
        return new CreateModel.Document.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId
            },
            NodeDetails = nodeDetailsMapper.Map(source.NodeDetailsForUpdate),
            SimpleTextNodeDetails = new CreateModel.SimpleTextNodeDetails {
                Text = textService.FormatText(source.SimpleTextNodeDetails.Text),
                Teaser = textService.FormatTeaser(source.SimpleTextNodeDetails.Text)
            },
            DocumentDetails = new CreateModel.DocumentDetails {
                DocumentTypeId = source.DocumentDetails.DocumentTypeId,
                Published = source.DocumentDetails.Published,
                SourceUrl = source.DocumentDetails.SourceUrl,
            },
        };
    }
}
