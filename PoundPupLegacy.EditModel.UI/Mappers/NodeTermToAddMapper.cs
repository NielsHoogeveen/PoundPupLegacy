using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NodeTermToAddMapper : IEnumerableMapper<Tags, NodeTermToAdd>
{
    public IEnumerable<NodeTermToAdd> Map(IEnumerable<Tags> source)
    {
        foreach (var tag in source.SelectMany(x => x.Entries)) {
            if(!tag.NodeId.HasValue)
                continue;
            if(tag.HasBeenDeleted)
                continue;
            yield return new NodeTermToAdd {
                NodeId = tag.NodeId.Value,
                TermId = tag.TermId
            };
        }
    }
}
