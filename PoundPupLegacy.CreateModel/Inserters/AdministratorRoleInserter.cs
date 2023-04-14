namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class AdministratorRoleInserterFactory : DatabaseInserterFactory<AdministratorRole, AdministratorRoleInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };

    public override string TableName => "administrator_role";

}
internal class AdministratorRoleInserter : DatabaseInserter<AdministratorRole>
{

    public AdministratorRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(AdministratorRole item)
    {
        if (item.Id is null)
            throw new ArgumentNullException(nameof(item.Id));
        if (item.UserGroupId is null)
            throw new ArgumentNullException(nameof(item.UserGroupId));

        return new ParameterValue[] {
            ParameterValue.Create(AdministratorRoleInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(AdministratorRoleInserterFactory.UserGroupId, item.UserGroupId.Value)
        };
    }
}
