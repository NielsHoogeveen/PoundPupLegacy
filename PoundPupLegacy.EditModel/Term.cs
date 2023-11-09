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
        private List<ParentTermListItem> _parentTerms = new();
        public required List<ParentTermListItem> ParentTerms { 
            get => _parentTerms; 
            init {
                if(value is not null) {
                    _parentTerms = value;
                }
            } 
        }

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