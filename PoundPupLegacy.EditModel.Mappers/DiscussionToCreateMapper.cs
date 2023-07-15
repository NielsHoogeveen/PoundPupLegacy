using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Mappers;

internal class DiscussionToCreateMapper(
    ITextService textService,
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailsMapper
    ) : IMapper<Discussion.ToCreate, DomainModel.Discussion.ToCreate>
{
    public DomainModel.Discussion.ToCreate Map(Discussion.ToCreate source)
    {
        return new DomainModel.Discussion.ToCreate {
            Identification = new Identification.Possible {
                Id = null
            },
            NodeDetails = nodeDetailsMapper.Map(source.NodeDetailsForCreate),
            SimpleTextNodeDetails = new DomainModel.SimpleTextNodeDetails {
                Text = textService.FormatText(source.SimpleTextNodeDetails.Text),
                Teaser = textService.FormatTeaser(source.SimpleTextNodeDetails.Text)
            }
        };
    }
}
