using PoundPupLegacy.CreateModel;

namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class NodeTermToAddForCreateMapper : IEnumerableMapper<Tags.ToCreate, int>
{
    public IEnumerable<int> Map(IEnumerable<Tags.ToCreate> source)
    {
        foreach (var tag in source.SelectMany(x => x.EntriesToCreate)) {
            if(tag is NodeTerm.ForCreate newNodeTerm) {
                yield return tag.TermId;
            }
        }
    }
}
