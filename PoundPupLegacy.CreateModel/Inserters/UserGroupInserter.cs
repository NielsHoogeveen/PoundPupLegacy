namespace PoundPupLegacy.CreateModel.Inserters;

using Request = UserGroup;

public class UserGroupInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    private static readonly NullCheckingIntegerDatabaseParameter AdministratorRoleId = new() { Name = "administrator_role_id" };

    public override string TableName => "user_group";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Name, request.Name),
            ParameterValue.Create(Description, request.Description),
            ParameterValue.Create(AdministratorRoleId, request.AdministratorRole?.Id)
        };
    }
}
