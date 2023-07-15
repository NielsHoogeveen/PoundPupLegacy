using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Mappers;
internal class DiscussionToUpdateMapper(
    ITextService textService,
    IMapper<NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper
    ) : IMapper<Discussion.ToUpdate, CreateModel.Discussion.ToUpdate>
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
