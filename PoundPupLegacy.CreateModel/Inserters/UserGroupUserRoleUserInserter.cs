namespace PoundPupLegacy.CreateModel.Inserters;

using Request = UserGroupUserRoleUser;

internal sealed class UserGroupUserRoleUserInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserRoleId = new() { Name = "user_role_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };

    public override string TableName => "user_group_user_role_user";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UserGroupId, item.UserGroupId),
            ParameterValue.Create(UserRoleId, item.UserRoleId),
            ParameterValue.Create(UserId, item.UserId),
        };
    }
}
