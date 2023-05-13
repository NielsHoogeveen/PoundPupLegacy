namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(NewInterOrganizationalNewFromRelation))]
public partial class NewInterOrganizationalNewFromRelationJsonContext : JsonSerializerContext { }

public record NewInterOrganizationalNewFromRelation : InterOrganizationalRelationBase, NewInterOrganizationalRelation
{
}
