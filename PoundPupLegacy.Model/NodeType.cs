namespace PoundPupLegacy.Model;

public interface NodeType: Identifiable
{
    string Name { get; }
    string Description { get; }
}
