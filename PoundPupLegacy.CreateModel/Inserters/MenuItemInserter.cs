namespace PoundPupLegacy.CreateModel.Inserters;

using Request = MenuItem;

internal sealed class MenuItemInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request>
{
    internal static NonNullableDoubleDatabaseParameter Weight = new() { Name = "weight" };
    public override string TableName => "menu_item";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Weight, request.Weight)
        };
    }
}
