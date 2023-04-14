namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = AccessRolePrivilegeInserterFactory;
using Request = AccessRolePrivilege;
using Inserter = AccessRolePrivilegeInserter;

internal sealed class AccessRolePrivilegeInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter AccessRoleId = new() { Name = "access_role_id" };
    internal static NonNullableIntegerDatabaseParameter ActionId = new() { Name = "action_id" };
    public override string TableName => "access_role_privilege";
}
internal sealed class AccessRolePrivilegeInserter : DatabaseInserter<Request>
{

    public AccessRolePrivilegeInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.AccessRoleId, request.AccessRoleId),
            ParameterValue.Create(Factory.ActionId, request.ActionId)
        };
    }
}
