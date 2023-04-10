namespace PoundPupLegacy.CreateModel.Inserters;

public class UserGroupInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<UserGroup, UserGroupInserter>
{
    internal static AutoGenerateIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NonNullableIntegerDatabaseParameter AdministratorRoleId = new() { Name = "administrator_role_id" };

    public override string TableName => "user_group";

}
public class UserGroupInserter : ConditionalAutoGenerateIdDatabaseInserter<UserGroup>
{
    public UserGroupInserter(NpgsqlCommand command, NpgsqlCommand generateIdInsertCommand) : base(command, generateIdInsertCommand)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(UserGroup item)
    {
        if (item.AdministratorRole.Id is null)
            throw new ArgumentNullException(nameof(item.AdministratorRole.Id));

        return new ParameterValue[] {
            ParameterValue.Create(UserGroupInserterFactory.Id, item.Id),
            ParameterValue.Create(UserGroupInserterFactory.Name, item.Name),
            ParameterValue.Create(UserGroupInserterFactory.Description, item.Description),
            ParameterValue.Create(UserGroupInserterFactory.AdministratorRoleId, item.AdministratorRole.Id.Value)
        };
    }
}
