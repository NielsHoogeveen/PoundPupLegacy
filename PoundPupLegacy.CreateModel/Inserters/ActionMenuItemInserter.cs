namespace PoundPupLegacy.CreateModel.Inserters;

using Request = ActionMenuItem;

internal sealed class ActionMenuItemInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static TrimmingNonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter ActionId = new() { Name = "action_id" };

    public override string TableName => "action_menu_item";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Name, request.Name),
            ParameterValue.Create(ActionId, request.ActionId)
        };
    }
}
