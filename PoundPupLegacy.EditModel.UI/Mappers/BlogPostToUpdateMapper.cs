using PoundPupLegacy.EditModel.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class BlogPostToUpdateMapper(
    ITextService textService,
    IMapper<EditModel.NodeDetails.ForUpdate, CreateModel.NodeDetails.ForUpdate> nodeDetailsMapper
    ) : IMapper<EditModel.BlogPost.ToUpdate, CreateModel.BlogPost.ToUpdate>
{
    public CreateModel.BlogPost.ToUpdate Map(BlogPost.ToUpdate source)
    {
        return new CreateModel.BlogPost.ToUpdate {
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
