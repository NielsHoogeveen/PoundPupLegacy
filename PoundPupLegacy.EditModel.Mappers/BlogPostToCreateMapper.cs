namespace PoundPupLegacy.EditModel.Mappers;

internal class BlogPostToCreateMapper(
    ITextService textService,
    IMapper<NodeDetails.ForCreate, CreateModel.NodeDetails.ForCreate> nodeDetailsMapper
    ) : IMapper<BlogPost.ToCreate, CreateModel.BlogPost.ToCreate>
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
