namespace PoundPupLegacy.EditModel;

[JsonSerializable(typeof(NewInterOrganizationalExistingFromRelation))]
public partial class NewInterOrganizationalExistingFromRelationJsonContext : JsonSerializerContext { }


public record NewInterOrganizationalExistingFromRelation : InterOrganizationalRelationBase, NewInterOrganizationalRelation
{

}
