using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class DiscussionToCreateMapper(
    ITextService textService,
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper
    ) : IMapper<EditModel.Discussion.ToCreate, CreateModel.Discussion.ToCreate>
{
    public CreateModel.Discussion.ToCreate Map(Discussion.ToCreate source)
    {
        return new CreateModel.Discussion.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = nodeDetailsMapper.Map(source.NodeDetailsForCreate),
            SimpleTextNodeDetails = new CreateModel.SimpleTextNodeDetails {
                Text = textService.FormatText(source.SimpleTextNodeDetails.Text),
                Teaser = textService.FormatTeaser(source.SimpleTextNodeDetails.Text)
            }
        };
    }
}
