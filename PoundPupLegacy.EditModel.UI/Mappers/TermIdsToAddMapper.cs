namespace PoundPupLegacy.EditModel.UI.Mappers;

internal class TermIdsToAddMapper : IEnumerableMapper<Tags, int>
{
    public IEnumerable<int> Map(IEnumerable<Tags> source)
    {
        foreach (var tag in source.SelectMany(x => x.Entries)) {
            if(tag is NodeTerm.ForCreate)
                yield return tag.TermId;
        }
    }
}
