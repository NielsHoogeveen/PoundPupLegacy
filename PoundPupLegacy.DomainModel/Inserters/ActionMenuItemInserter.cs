namespace PoundPupLegacy.DomainModel.Inserters;

using Request = ActionMenuItem;

internal sealed class ActionMenuItemInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly TrimmingNonNullableStringDatabaseParameter Name = new() { Name = "name" };
    private static readonly NonNullableIntegerDatabaseParameter ActionId = new() { Name = "action_id" };

    public override string TableName => "action_menu_item";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Name, request.Name),
            ParameterValue.Create(ActionId, request.ActionId)
        };
    }
}
