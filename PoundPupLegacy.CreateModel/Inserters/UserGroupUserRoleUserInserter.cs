namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = UserGroupUserRoleUserInserterFactory;
using Request = UserGroupUserRoleUser;
using Inserter = UserGroupUserRoleUserInserter;

internal sealed class UserGroupUserRoleUserInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };
    internal static NonNullableIntegerDatabaseParameter UserRoleId = new() { Name = "user_role_id" };
    internal static NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };

    public override string TableName => "user_group_user_role_user";
}
internal sealed class UserGroupUserRoleUserInserter : DatabaseInserter<Request>
{
    public UserGroupUserRoleUserInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.UserGroupId, item.UserGroupId),
            ParameterValue.Create(Factory.UserRoleId, item.UserRoleId),
            ParameterValue.Create(Factory.UserId, item.UserId),
        };
    }
}
