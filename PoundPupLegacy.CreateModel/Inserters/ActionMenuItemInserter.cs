namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = ActionMenuItemInserterFactory;
using Request = ActionMenuItem;
using Inserter = ActionMenuItemInserter;

internal sealed class ActionMenuItemInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static TrimmingNonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter ActionId = new() { Name = "action_id" };

    public override string TableName => "action_menu_item";

}
internal sealed class ActionMenuItemInserter : IdentifiableDatabaseInserter<Request>
{
    public ActionMenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Name, request.Name),
            ParameterValue.Create(Factory.ActionId, request.ActionId)
        };
    }
}
