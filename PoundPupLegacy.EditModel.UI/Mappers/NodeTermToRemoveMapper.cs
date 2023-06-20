using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NodeTermToRemoveMapper : IEnumerableMapper<Tags.ToUpdate, NodeTermToRemove>
{
    public IEnumerable<NodeTermToRemove> Map(IEnumerable<Tags.ToUpdate> source)
    {
        foreach (var tag in source.SelectMany(x => x.EntriesToUpdate)) {
            if(tag is NodeTerm.ForUpdate existingNodeTerm && existingNodeTerm.NodeTermStatus == NodeTermStatus.Removed) {
                yield return new NodeTermToRemove {
                    NodeId = existingNodeTerm.NodeId,
                    TermId = existingNodeTerm.TermId
                };
            }
        }
    }
}
