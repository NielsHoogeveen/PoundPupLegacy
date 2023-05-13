namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(NewInterOrganizationalNewToRelation))]
public partial class NewInterOrganizationalNewToRelationJsonContext : JsonSerializerContext { }

public record NewInterOrganizationalNewToRelation : InterOrganizationalRelationBase, NewInterOrganizationalRelation
{
}
