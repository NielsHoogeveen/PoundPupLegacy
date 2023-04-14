namespace PoundPupLegacy.CreateModel;

public sealed record BasicNodeType : NodeType
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
    public required string Description { get; init; }

    public required bool AuthorSpecific { get; init; }

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
