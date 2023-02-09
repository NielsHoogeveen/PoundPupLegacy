namespace PoundPupLegacy.Model;

public sealed record NodeType
{
    public int Id { get; }
    public string Name { get; }
    public string Description { get; }

    public NodeType(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }
}