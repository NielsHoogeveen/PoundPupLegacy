namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class MenuItemInserter : DatabaseInserter<MenuItem>, IDatabaseInserter<MenuItem>
{

    private const string WEIGHT = "weight";

    public static async Task<DatabaseInserter<MenuItem>> CreateAsync(NpgsqlConnection connection)
    {
        var collumnDefinitions = new ColumnDefinition[]
        {
            new ColumnDefinition{
                Name = WEIGHT,
                NpgsqlDbType = NpgsqlDbType.Double
            },
        };

        var command = await CreateIdentityInsertStatementAsync(
            connection,
            "menu_item",
            collumnDefinitions
        );
        return new MenuItemInserter(command);
    }

    internal MenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(MenuItem menuItem)
    {
        if (menuItem.Id != null) {
            throw new Exception($"Id of menu item needs to be null");
        }
        WriteValue(menuItem.Weight, WEIGHT);
        menuItem.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception("No id was generated for menu item")
        };
    }
}
