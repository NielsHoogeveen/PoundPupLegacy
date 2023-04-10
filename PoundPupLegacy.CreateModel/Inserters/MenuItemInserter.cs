namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class MenuItemInserterFactory : DatabaseInserterFactory<MenuItem>
{
    internal static NonNullableDoubleDatabaseParameter Weight = new() { Name = "weight" };

    public override async Task<IDatabaseInserter<MenuItem>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateIdentityInsertStatementAsync(
            postgresConnection,
            "menu_item",
            new DatabaseParameter[]
            {
                Weight
            }
        );
        return new MenuItemInserter(command);
    }

}
internal sealed class MenuItemInserter : DatabaseInserter<MenuItem>
{
    internal MenuItemInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(MenuItem menuItem)
    {
        if (menuItem.Id is not null) {
            throw new Exception($"Id of menu item needs to be null");
        }
        Set(MenuItemInserterFactory.Weight, menuItem.Weight);
        menuItem.Id = await _command.ExecuteScalarAsync() switch {
            long i => (int)i,
            _ => throw new Exception("No id was generated for menu item")
        };
    }
}
