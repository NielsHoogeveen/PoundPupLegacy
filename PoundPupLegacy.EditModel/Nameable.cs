namespace PoundPupLegacy.EditModel;

public interface Nameable : Node
{
    string Description { get; set; }
    List<Term> Terms { get; }
}
