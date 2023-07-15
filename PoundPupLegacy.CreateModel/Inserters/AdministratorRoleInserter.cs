namespace PoundPupLegacy.DomainModel.Inserters;

using Request = AdministratorRole;

internal sealed class AdministratorRoleInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NullCheckingIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };

    public override string TableName => "administrator_role";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(UserGroupId, request.UserGroupId)
        };
    }
}
