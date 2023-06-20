using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NodeTermToAddForUpdateMapper : IEnumerableMapper<Tags.ToUpdate, ResolvedNodeTermToAdd>
{
    public IEnumerable<ResolvedNodeTermToAdd> Map(IEnumerable<Tags.ToUpdate> source)
    {
        foreach (var tag in source.SelectMany(x => x.EntriesToUpdate).Where(x => x.NodeTermStatus == NodeTermStatus.New)) {
            if(tag is NodeTerm.ForUpdate newNodeTerm) {
                yield return new ResolvedNodeTermToAdd {
                    NodeId = tag.NodeId,
                    TermId = tag.TermId
                };
            }
        }
    }
}
