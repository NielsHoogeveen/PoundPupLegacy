namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = UserGroupInserterFactory;
using Request = UserGroup;
using Inserter = UserGroupInserter;

public class UserGroupInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NullCheckingIntegerDatabaseParameter AdministratorRoleId = new() { Name = "administrator_role_id" };

    public override string TableName => "user_group";

}
public class UserGroupInserter : ConditionalAutoGenerateIdDatabaseInserter<Request>
{
    public UserGroupInserter(NpgsqlCommand command, NpgsqlCommand generateIdInsertCommand) : base(command, generateIdInsertCommand)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Name, request.Name),
            ParameterValue.Create(Factory.Description, request.Description),
            ParameterValue.Create(Factory.AdministratorRoleId, request.AdministratorRole?.Id)
        };
    }
}
