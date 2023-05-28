namespace PoundPupLegacy.EditModel;
public interface Nameable: Node
{
    public NameableDetails NameableDetails { get; }

}

public sealed record NameableDetails 
{
    public required string Description { get; set; }

    private List<Term> terms = new();

    public List<Term> Terms {
        get => terms;
        init {
            if (value is not null) {
                terms = value;
            }
        }
    }
    public required int VocabularyIdTagging { get; init; }
}

