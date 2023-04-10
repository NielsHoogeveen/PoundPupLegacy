namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class UserGroupUserRoleUserInserterFactory : BasicDatabaseInserterFactory<UserGroupUserRoleUser, UserGroupUserRoleUserInserter>
{
    internal static NonNullableIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };
    internal static NonNullableIntegerDatabaseParameter UserRoleId = new() { Name = "user_role_id" };
    internal static NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };

    public override string TableName => "user_group_user_role_user";
}
internal sealed class UserGroupUserRoleUserInserter : BasicDatabaseInserter<UserGroupUserRoleUser>
{
    public UserGroupUserRoleUserInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(UserGroupUserRoleUser item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UserGroupUserRoleUserInserterFactory.UserGroupId, item.UserGroupId),
            ParameterValue.Create(UserGroupUserRoleUserInserterFactory.UserRoleId, item.UserRoleId),
            ParameterValue.Create(UserGroupUserRoleUserInserterFactory.UserId, item.UserId),
        };
    }
}
