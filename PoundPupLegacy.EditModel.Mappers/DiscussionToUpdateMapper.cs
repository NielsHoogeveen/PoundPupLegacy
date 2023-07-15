using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Mappers;
internal class DiscussionToUpdateMapper(
    ITextService textService,
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailsMapper
    ) : IMapper<Discussion.ToUpdate, DomainModel.Discussion.ToUpdate>
{
    public DomainModel.Discussion.ToUpdate Map(Discussion.ToUpdate source)
    {
        return new DomainModel.Discussion.ToUpdate {
            Identification = new Identification.Certain {
                Id = source.NodeIdentification.NodeId
            },
            NodeDetails = nodeDetailsMapper.Map(source.NodeDetailsForUpdate),
            SimpleTextNodeDetails = new DomainModel.SimpleTextNodeDetails {
                Text = textService.FormatText(source.SimpleTextNodeDetails.Text),
                Teaser = textService.FormatTeaser(source.SimpleTextNodeDetails.Text)
            }
        };
    }
}
