namespace PoundPupLegacy.CreateModel;

public sealed record BasicNodeType : NewNodeType, NodeTypeToAdd
{
    public static BasicNodeType Create(int id, string name, string description, bool authorSpecific)
    {
        return new BasicNodeType {
            Identification = new Identification.Possible { 
                Id = id
            },
            Name = name,
            Description = description,
            AuthorSpecific = authorSpecific
        };
    }
}
