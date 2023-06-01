using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.EditModel.UI.Mappers;
internal class DiscussionToUpdateMapper(
    ITextService textService,
    IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper
    ) : IMapper<EditModel.Discussion.ToUpdate, CreateModel.Discussion.ToUpdate>
{
    public CreateModel.Discussion.ToUpdate Map(Discussion.ToUpdate source)
    {
        return new CreateModel.Discussion.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId
            },
            NodeDetails = nodeDetailsMapper.Map(source.NodeDetailsForUpdate),
            SimpleTextNodeDetails = new CreateModel.SimpleTextNodeDetails {
                Text = textService.FormatText(source.SimpleTextNodeDetails.Text),
                Teaser = textService.FormatTeaser(source.SimpleTextNodeDetails.Text)
            }
        };
    }
}
