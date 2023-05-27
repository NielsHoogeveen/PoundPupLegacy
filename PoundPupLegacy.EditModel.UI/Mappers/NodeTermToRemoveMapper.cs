using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NodeTermToRemoveMapper : IEnumerableMapper<Tags, NodeTermToRemove>
{
    public IEnumerable<NodeTermToRemove> Map(IEnumerable<Tags> source)
    {
        foreach (var tag in source.SelectMany(x => x.Entries)) {
            if(tag is NodeTerm.ExistingNodeTerm existingNodeTerm && existingNodeTerm.HasBeenDeleted) {
                yield return new NodeTermToRemove {
                    NodeId = existingNodeTerm.NodeId,
                    TermId = existingNodeTerm.TermId
                };
            }
        }
    }
}
