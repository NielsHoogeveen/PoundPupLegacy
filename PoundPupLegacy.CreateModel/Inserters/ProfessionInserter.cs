namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ProfessionInserterFactory : DatabaseInserterFactory<Profession>
{
    public override async Task<IDatabaseInserter<Profession>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "profession",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ProfessionInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ProfessionInserter.HAS_CONCRETE_SUBTYPE,
                    NpgsqlDbType = NpgsqlDbType.Boolean
                },
            }
        );
        return new ProfessionInserter(command);
    }
}
internal sealed class ProfessionInserter : DatabaseInserter<Profession>
{
    internal const string ID = "id";
    internal const string HAS_CONCRETE_SUBTYPE = "has_concrete_subtype";

    internal ProfessionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Profession profession)
    {
        if (profession.Id is null)
            throw new NullReferenceException();

        WriteValue(profession.Id, ID);
        WriteValue(profession.HasConcreteSubtype, HAS_CONCRETE_SUBTYPE);
        await _command.ExecuteNonQueryAsync();
    }
}
