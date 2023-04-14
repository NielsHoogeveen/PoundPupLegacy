namespace PoundPupLegacy.CreateModel.Inserters;

public class UserRoleInserterFactory : DatabaseInserterFactory<UserRole, UserRoleInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "user_role";

}
public class UserRoleInserter : DatabaseInserter<UserRole>
{
    public UserRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(UserRole item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        if (item.UserGroupId is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(UserRoleInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(UserRoleInserterFactory.UserGroupId, item.UserGroupId.Value),
            ParameterValue.Create(UserRoleInserterFactory.Name, item.Name),
        };
    }
}
