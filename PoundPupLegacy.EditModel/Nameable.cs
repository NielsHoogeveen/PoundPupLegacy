namespace PoundPupLegacy.EditModel;
public interface Nameable: Node
{
    public NameableDetails NameableDetails { get; }

}

public sealed record NameableDetails
{
    public required int? TopicId { get; init; }
    public required string Name { get; set; }
    public required string Description { get; set; }

    public required VocabularyListItem[] Vocabularies { get; init; }

    private List<Term.WithHierarchy> termsWithHierarchy = new();
    public required List<Term.WithHierarchy> TermsWithHierarchy {
        get => termsWithHierarchy;
        init {
            if (value is not null) {
                termsWithHierarchy = value;
            }
        }
    }

    private List<Term.WithoutHierarchy> termsWithoutHierarchy = new();
    public required List<Term.WithoutHierarchy> TermsWithoutHierarchy {
        get => termsWithoutHierarchy;
        init {
            if (value is not null) {
                termsWithoutHierarchy = value;
            }
        }
    }

    public List<Term> Terms { 
        get {
            var lst = new List<Term>();
            lst.AddRange(TermsWithoutHierarchy);
            lst.AddRange(TermsWithHierarchy);
            return lst;
        }
    }
    public required int VocabularyId { get; set; }
}

