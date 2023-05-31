using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NodeTermToAddForUpdateMapper : IEnumerableMapper<Tags.ToUpdate, ResolvedNodeTermToAdd>
{
    public IEnumerable<ResolvedNodeTermToAdd> Map(IEnumerable<Tags.ToUpdate> source)
    {
        foreach (var tag in source.SelectMany(x => x.EntriesToUpdate)) {
            if(tag is NodeTerm.ForUpdate newNodeTerm) {
                yield return new ResolvedNodeTermToAdd {
                    NodeId = tag.NodeId,
                    TermId = tag.TermId
                };
            }
        }
    }
}
