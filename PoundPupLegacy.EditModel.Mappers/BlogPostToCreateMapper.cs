namespace PoundPupLegacy.EditModel.Mappers;

internal class BlogPostToCreateMapper(
    ITextService textService,
    IMapper<NodeDetails.ForCreate, DomainModel.NodeDetails.ForCreate> nodeDetailsMapper
    ) : IMapper<BlogPost.ToCreate, DomainModel.BlogPost.ToCreate>
{
    public DomainModel.BlogPost.ToCreate Map(BlogPost.ToCreate source)
    {
        return new DomainModel.BlogPost.ToCreate {
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
