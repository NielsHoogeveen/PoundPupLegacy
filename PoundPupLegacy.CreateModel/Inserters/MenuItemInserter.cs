namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = MenuItemInserterFactory;
using Request = MenuItem;
using Inserter = MenuItemInserter;

internal sealed class MenuItemInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableDoubleDatabaseParameter Weight = new() { Name = "weight" };
    public override string TableName => "menu_item";

}
internal sealed class MenuItemInserter : AutoGenerateIdDatabaseInserter<MenuItem>
{
    public MenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Weight, request.Weight)
        };
    }
}
