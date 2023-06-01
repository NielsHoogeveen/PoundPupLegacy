using PoundPupLegacy.EditModel.UI.Services;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class BlogPostToCreateMapper(
    ITextService textService,
    IMapper<EditModel.NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper
    ) : IMapper<EditModel.BlogPost.ToCreate, CreateModel.BlogPost.ToCreate>
{
    public CreateModel.BlogPost.ToCreate Map(BlogPost.ToCreate source)
    {
        return new CreateModel.BlogPost.ToCreate {
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
