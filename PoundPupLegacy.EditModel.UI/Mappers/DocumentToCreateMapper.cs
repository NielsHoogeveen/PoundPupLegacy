using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class DocumentToCreateMapper(
    ITextService textService,
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper
    ) : IMapper<EditModel.Document.ToCreate, CreateModel.Document.ToCreate>
{
    public CreateModel.Document.ToCreate Map(Document.ToCreate source)
    {
        return new CreateModel.Document.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = nodeDetailsMapper.Map(source.NodeDetailsForCreate),
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
