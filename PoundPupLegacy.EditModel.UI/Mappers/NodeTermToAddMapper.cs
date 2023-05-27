using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NodeTermToAddMapper : IEnumerableMapper<Tags, NodeTermToAdd>
{
    public IEnumerable<NodeTermToAdd> Map(IEnumerable<Tags> source)
    {
        foreach (var tag in source.SelectMany(x => x.Entries)) {
            if(tag is NodeTerm.NewNodeTerm newNodeTerm) {
                yield return new NodeTermToAdd {
                    TermId = tag.TermId
                };
            }
        }
    }
}
