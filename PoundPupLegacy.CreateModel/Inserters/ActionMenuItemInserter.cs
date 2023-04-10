namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ActionMenuItemInserterFactory : BasicDatabaseInserterFactory<ActionMenuItem, ActionMenuItemInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter ActionId = new() { Name = "action_id" };

    public override string TableName => "action_menu_item";

}
internal sealed class ActionMenuItemInserter : BasicDatabaseInserter<ActionMenuItem>
{
    public ActionMenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }
    public override IEnumerable<ParameterValue> GetParameterValues(ActionMenuItem item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(ActionMenuItemInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(ActionMenuItemInserterFactory.Name, item.Name.Trim()),
            ParameterValue.Create(ActionMenuItemInserterFactory.ActionId, item.ActionId)
        };
    }
}
