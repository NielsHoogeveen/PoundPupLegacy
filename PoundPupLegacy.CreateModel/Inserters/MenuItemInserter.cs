namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class MenuItemInserterFactory : AutoGenerateIdDatabaseInserterFactory<MenuItem, MenuItemInserter>
{
    internal static NonNullableDoubleDatabaseParameter Weight = new() { Name = "weight" };
    public override string TableName => "menu_item";

}
internal sealed class MenuItemInserter : AutoGenerateIdDatabaseInserter<MenuItem>
{
    public MenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(MenuItem item)
    {
        if (item.Id.HasValue) {
            throw new Exception($"menu item id should be null upon creation");
        }
        return new ParameterValue[] {
            ParameterValue.Create(MenuItemInserterFactory.Weight, item.Weight)
        };
    }
}
