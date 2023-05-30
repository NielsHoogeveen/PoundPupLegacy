namespace PoundPupLegacy.CreateModel;

public sealed record BasicNameableType : NameableTypeBase, NameableTypeToAdd
{
    public static BasicNameableType Create(int id, string name, string description, bool authorSpecific, string tagLabelName)
    {
        return new BasicNameableType {
            IdentificationForCreate = new Identification.IdentificationForCreate {
                Id = id,
            },
            Name = name,
            Description = description,
            AuthorSpecific = authorSpecific,
            TagLabelName = tagLabelName
        };
    }
}
