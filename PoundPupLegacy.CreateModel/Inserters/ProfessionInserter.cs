namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ProfessionInserterFactory : DatabaseInserterFactory<Profession>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableBooleanDatabaseParameter HasConcreteSubtype = new() { Name = "has_concrete_subtype" };

    public override async Task<IDatabaseInserter<Profession>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "profession",
            new DatabaseParameter[] {
                Id,
                HasConcreteSubtype
            }
        );
        return new ProfessionInserter(command);
    }
}
internal sealed class ProfessionInserter : DatabaseInserter<Profession>
{

    internal ProfessionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Profession profession)
    {
        if (profession.Id is null)
            throw new NullReferenceException();

        Set(ProfessionInserterFactory.Id, profession.Id.Value);
        Set(ProfessionInserterFactory.HasConcreteSubtype, profession.HasConcreteSubtype);
        await _command.ExecuteNonQueryAsync();
    }
}
