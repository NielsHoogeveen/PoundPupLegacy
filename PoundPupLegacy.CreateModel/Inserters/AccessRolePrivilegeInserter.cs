namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class AccessRolePrivilegeInserterFactory : BasicDatabaseInserterFactory<AccessRolePrivilege, AccessRolePrivilegeInserter>
{
    internal static NonNullableIntegerDatabaseParameter AccessRoleId = new() { Name = "access_role_id" };
    internal static NonNullableIntegerDatabaseParameter ActionId = new() { Name = "action_id" };
    public override string TableName => "access_role_privilege";
}
internal sealed class AccessRolePrivilegeInserter : BasicDatabaseInserter<AccessRolePrivilege>
{

    public AccessRolePrivilegeInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(AccessRolePrivilege item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(AccessRolePrivilegeInserterFactory.AccessRoleId, item.AccessRoleId),
            ParameterValue.Create(AccessRolePrivilegeInserterFactory.ActionId, item.ActionId)
        };
    }
}
