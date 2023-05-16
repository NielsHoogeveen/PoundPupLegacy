namespace PoundPupLegacy.EditModel;

public abstract record NameableBase: NodeBase, Nameable
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

}
public interface Nameable : Node
{
    string Description { get; set; }
    List<Term> Terms { get; }
}
