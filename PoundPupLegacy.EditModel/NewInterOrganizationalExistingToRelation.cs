namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(NewInterOrganizationalExistingToRelation))]
public partial class NewInterOrganizationalExistingToRelationJsonContext : JsonSerializerContext { }

public record NewInterOrganizationalExistingToRelation : InterOrganizationalRelationBase, NewInterOrganizationalRelation
{
}
