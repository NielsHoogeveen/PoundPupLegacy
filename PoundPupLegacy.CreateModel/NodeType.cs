namespace PoundPupLegacy.CreateModel;

public interface NodeType : Identifiable
{
    string Name { get; }
    string Description { get; }
    bool AuthorSpecific { get; }
}
