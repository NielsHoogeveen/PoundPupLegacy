namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CaseInserterFactory : DatabaseInserterFactory<Case>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NullableTimeStampRangeDatabaseParameter FuzzyDate = new() { Name = "fuzzy_date" };

    public override async Task<IDatabaseInserter<Case>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "case",
            new DatabaseParameter[] {
                Id,
                Description,
                FuzzyDate
            }
        );
        return new CaseInserter(command);

    }

}
internal sealed class CaseInserter : DatabaseInserter<Case>
{
    internal CaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Case @case)
    {
        if (@case.Id is null)
            throw new NullReferenceException();
        Set(CaseInserterFactory.Id, @case.Id.Value);
        Set(CaseInserterFactory.Description,@case.Description);
        Set(CaseInserterFactory.FuzzyDate, @case.Date);
        await _command.ExecuteNonQueryAsync();
    }
}
