namespace PoundPupLegacy.CreateModel.Inserters;

using Request = AccessRolePrivilege;

internal sealed class AccessRolePrivilegeInserterFactory : BasicDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter AccessRoleId = new() { Name = "access_role_id" };
    internal static NonNullableIntegerDatabaseParameter ActionId = new() { Name = "action_id" };
    public override string TableName => "access_role_privilege";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(AccessRoleId, request.AccessRoleId),
            ParameterValue.Create(ActionId, request.ActionId)
        };
    }
}
