namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = UserRoleInserterFactory;
using Request = UserRole;
using Inserter = UserRoleInserter;

public class UserRoleInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullCheckingIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "user_role";

}
public class UserRoleInserter : IdentifiableDatabaseInserter<Request>
{
    public UserRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.UserGroupId, request.UserGroupId),
            ParameterValue.Create(Factory.Name, request.Name),
        };
    }
}
