namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(Term))]
public partial class TermJsonContext : JsonSerializerContext { }

public abstract record Term
{
    private Term() { }
    public required int? Id { get; set; }

    public required string Name { get; set; }

    public required int? NodeId { get; set; }

    public required int VocabularyId { get; set; }

    public required string VocabularyName { get; set; }

    public required bool IsMainTerm { get; set; }

    public sealed record WithHierarchy: Term
    {
        public required List<ParentTermListItem> ParentTerms { get; init; }

        private List<int>? newParentIds = null;
        public List<int> NewParentIds {
            get {
                if(newParentIds == null) {
                    newParentIds = ParentTerms.Select(x => x.Id).ToList();
                }
                return newParentIds;
            }
        }
    }
    public sealed record WithoutHierarchy: Term
    {

    }
}

public record ParentTermListItem: ListEditElement
{

}