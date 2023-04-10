namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DeportationCaseInserterFactory : DatabaseInserterFactory<DeportationCase>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableIntegerDatabaseParameter SubdivisionIdFrom = new() { Name = "subdivision_id_from" };
    internal static NullableIntegerDatabaseParameter CountryIdTo = new() { Name = "country_id_to" };

    public override async Task<IDatabaseInserter<DeportationCase>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "deportation_case",
            new DatabaseParameter[] {
                Id,
                SubdivisionIdFrom,
                CountryIdTo
            }
        );
        return new DeportationCaseInserter(command);
    }
}

internal sealed class DeportationCaseInserter : DatabaseInserter<DeportationCase>
{

    internal DeportationCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(DeportationCase deportationCase)
    {
        if (deportationCase.Id is null)
            throw new NullReferenceException();

        Set(DeportationCaseInserterFactory.Id, deportationCase.Id.Value);
        Set(DeportationCaseInserterFactory.SubdivisionIdFrom, deportationCase.SubdivisionIdFrom);
        Set(DeportationCaseInserterFactory.CountryIdTo, deportationCase.CountryIdTo);
        await _command.ExecuteNonQueryAsync();
    }
}
