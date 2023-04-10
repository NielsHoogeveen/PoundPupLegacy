namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ActInserterFactory : DatabaseInserterFactory<Act>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableDateTimeDatabaseParameter EnactmentDate = new() { Name = "enactment_date" };
    
    public override async Task<IDatabaseInserter<Act>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "act",
            new DatabaseParameter[] {
                Id, EnactmentDate
            }
        );
        return new ActInserter(command);

    }
}
internal sealed class ActInserter : DatabaseInserter<Act>
{
    internal ActInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Act act)
    {
        if (act.Id is null)
            throw new ArgumentNullException(nameof(act.Id));
        Set(ActInserterFactory.Id, act.Id.Value);
        Set(ActInserterFactory.EnactmentDate, act.EnactmentDate);
        await _command.ExecuteNonQueryAsync();
    }
}
