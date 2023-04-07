namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class MenuItemInserterFactory : DatabaseInserterFactory<MenuItem>
{
    public override async Task<IDatabaseInserter<MenuItem>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var collumnDefinitions = new ColumnDefinition[]
        {
            new ColumnDefinition{
                Name = MenuItemInserter.WEIGHT,
                NpgsqlDbType = NpgsqlDbType.Double
            },
        };

        var command = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "menu_item",
            collumnDefinitions
        );
        return new MenuItemInserter(command);
    }

}
internal sealed class MenuItemInserter : DatabaseInserter<MenuItem>
{

    internal const string WEIGHT = "weight";


    internal MenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(MenuItem menuItem)
    {
        if (menuItem.Id != null) {
            throw new Exception($"Id of menu item needs to be null");
        }
        SetParameter(menuItem.Weight, WEIGHT);
        menuItem.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception("No id was generated for menu item")
        };
    }
}
