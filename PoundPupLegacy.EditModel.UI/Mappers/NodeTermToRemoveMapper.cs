using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NodeTermToRemoveMapper : IEnumerableMapper<Tags, NodeTermToRemove>
{
    public IEnumerable<NodeTermToRemove> Map(IEnumerable<Tags> source)
    {
        foreach (var tag in source.SelectMany(x => x.Entries)) {
            if(!tag.NodeId.HasValue)
                continue;
            if(!tag.HasBeenDeleted)
                continue;
            yield return new NodeTermToRemove {
                NodeId = tag.NodeId.Value,
                TermId = tag.TermId
            };
        }
    }
}
