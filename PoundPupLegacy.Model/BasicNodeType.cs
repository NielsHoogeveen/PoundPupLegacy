namespace PoundPupLegacy.Model;

public sealed record BasicNodeType : NodeType
{
    public int? Id { get; set; }
    public string Name { get; }
    public string Description { get; }

    public BasicNodeType(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}
