namespace PoundPupLegacy.DomainModel.Inserters;

using Request = UserRoleToCreate;

public class UserRoleInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullCheckingIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "user_role";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UserGroupId, request.UserGroupId),
            ParameterValue.Create(Name, request.Name),
        };
    }
}
