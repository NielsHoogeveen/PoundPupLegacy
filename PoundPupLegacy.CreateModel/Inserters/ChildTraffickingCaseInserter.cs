namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ChildTraffickingCaseInserterFactory : DatabaseInserterFactory<ChildTraffickingCase>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableIntegerDatabaseParameter NumberOfChildrenInvolved = new() { Name = "number_of_children_involved" };
    internal static NonNullableIntegerDatabaseParameter CountryIdFrom = new() { Name = "country_id_from" };

    public override async Task<IDatabaseInserter<ChildTraffickingCase>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "child_trafficking_case",
            new DatabaseParameter[] {
                Id,
                NumberOfChildrenInvolved,
                CountryIdFrom
            }
        );
        return new ChildTraffickingCaseInserter(command);
    }
}

internal sealed class ChildTraffickingCaseInserter : DatabaseInserter<ChildTraffickingCase>
{
    internal ChildTraffickingCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(ChildTraffickingCase abuseCase)
    {
        if (abuseCase.Id is null)
            throw new NullReferenceException();

        Set(ChildTraffickingCaseInserterFactory.Id, abuseCase.Id.Value);
        Set(ChildTraffickingCaseInserterFactory.NumberOfChildrenInvolved, abuseCase.NumberOfChildrenInvolved);
        Set(ChildTraffickingCaseInserterFactory.CountryIdFrom, abuseCase.CountryIdFrom);
        await _command.ExecuteNonQueryAsync();
    }
}
