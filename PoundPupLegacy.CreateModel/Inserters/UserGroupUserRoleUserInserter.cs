using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = UserRoleUser;

internal sealed class UserRoleUserInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter UserRoleId = new() { Name = "user_role_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };

    public override string TableName => "user_role_user";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UserRoleId, item.UserRoleId),
            ParameterValue.Create(UserId, item.UserId),
        };
    }
}
