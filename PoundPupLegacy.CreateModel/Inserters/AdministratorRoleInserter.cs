namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = AdministratorRoleInserterFactory;
using Request = AdministratorRole;
using Inserter = AdministratorRoleInserter;

internal sealed class AdministratorRoleInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullCheckingIntegerDatabaseParameter UserGroupId = new() { Name = "user_group_id" };

    public override string TableName => "administrator_role";

}
internal class AdministratorRoleInserter : IdentifiableDatabaseInserter<Request>
{

    public AdministratorRoleInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.UserGroupId, request.UserGroupId)
        };
    }
}
