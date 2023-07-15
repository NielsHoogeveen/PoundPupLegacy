namespace PoundPupLegacy.EditModel.Mappers;

internal class BlogPostToUpdateMapper(
    ITextService textService,
    IMapper<NodeDetails.ForUpdate, DomainModel.NodeDetails.ForUpdate> nodeDetailsMapper
    ) : IMapper<BlogPost.ToUpdate, DomainModel.BlogPost.ToUpdate>
{
    public DomainModel.BlogPost.ToUpdate Map(BlogPost.ToUpdate source)
    {
        return new DomainModel.BlogPost.ToUpdate {
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
