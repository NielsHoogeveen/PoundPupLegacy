namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(ExistingInterOrganizationalFromRelation))]
public partial class ExistingInterOrganizationalToRelationJsonContext : JsonSerializerContext { }

public record ExistingInterOrganizationalToRelation: InterOrganizationalRelationBase, ExistingInterOrganizationalRelation
{
    public int NodeId { get; init; }

    public int UrlId { get; set; }

    public bool HasBeenDeleted { get; set; }

}
