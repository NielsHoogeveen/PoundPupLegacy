namespace PoundPupLegacy.CreateModel.Inserters;

using Request = UserRole;

public class UserRoleInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NullCheckingIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "user_role";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UserGroupId, request.UserGroupId),
            ParameterValue.Create(Name, request.Name),
        };
    }
}
