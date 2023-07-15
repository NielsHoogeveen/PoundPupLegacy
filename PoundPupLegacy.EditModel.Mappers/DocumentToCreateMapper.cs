using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Mappers;

internal class DocumentToCreateMapper(
    ITextService textService,
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailsMapper
    ) : IMapper<Document.ToCreate, DomainModel.Document.ToCreate>
{
    public DomainModel.Document.ToCreate Map(Document.ToCreate source)
    {
        return new DomainModel.Document.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = nodeDetailsMapper.Map(source.NodeDetailsForCreate),
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
