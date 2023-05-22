namespace PoundPupLegacy.CreateModel;

public sealed record BasicNodeType : NewNodeType, EventuallyIdentifiableNodeType
{
    public static BasicNodeType Create(int id, string name, string description, bool authorSpecific)
    {
        return new BasicNodeType {
            Id = id,
            Name = name,
            Description = description,
            AuthorSpecific = authorSpecific
        };
    }
}
