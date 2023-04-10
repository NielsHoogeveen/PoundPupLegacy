namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class NameableInserterFactory : DatabaseInserterFactory<Nameable>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NullableIntegerDatabaseParameter FileIdTileImage = new() { Name = "file_id_tile_image" };

    public override async Task<IDatabaseInserter<Nameable>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "nameable",
            new DatabaseParameter[] {
                Id,
                Description,
                FileIdTileImage
            }
        );
        return new NameableInserter(command);
    }
}
internal sealed class NameableInserter : DatabaseInserter<Nameable>
{
    internal NameableInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Nameable nameable)
    {
        if (nameable.Id is null)
            throw new NullReferenceException();

        Set(NameableInserterFactory.Id,nameable.Id.Value);
        Set(NameableInserterFactory.Description, nameable.Description);
        Set(NameableInserterFactory.FileIdTileImage,nameable.FileIdTileImage);
        await _command.ExecuteNonQueryAsync();
    }
}
