namespace PoundPupLegacy.CreateModel;

public sealed record BasicNameableType : NameableType
{
    public required int? Id { get; set; }
    public required string Name { get; init; }
    public required string Description { get; init; }

    public required bool AuthorSpecific { get; init; }

    public required string TagLabelName { get; init; }

    public static BasicNameableType Create(int id, string name, string description, bool authorSpecific, string tagLabelName)
    {
        return new BasicNameableType {
            Id = id,
            Name = name,
            Description = description,
            AuthorSpecific = authorSpecific,
            TagLabelName = tagLabelName
        };
    }
}
