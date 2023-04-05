namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ChildTraffickingCaseInserterFactory : DatabaseInserterFactory<ChildTraffickingCase>
{
    public override async Task<IDatabaseInserter<ChildTraffickingCase>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "child_trafficking_case",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ChildTraffickingCaseInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ChildTraffickingCaseInserter.NUMBER_OF_CHILDREN_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ChildTraffickingCaseInserter.COUNTRY_ID_FROM,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },

            }
        );
        return new ChildTraffickingCaseInserter(command);
    }
}

internal sealed class ChildTraffickingCaseInserter : DatabaseInserter<ChildTraffickingCase>
{
    internal const string ID = "id";
    internal const string NUMBER_OF_CHILDREN_INVOLVED = "number_of_children_involved";
    internal const string COUNTRY_ID_FROM = "country_id_from";

    internal ChildTraffickingCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(ChildTraffickingCase abuseCase)
    {
        if (abuseCase.Id is null)
            throw new NullReferenceException();

        WriteValue(abuseCase.Id, ID);
        WriteNullableValue(abuseCase.NumberOfChildrenInvolved, NUMBER_OF_CHILDREN_INVOLVED);
        WriteValue(abuseCase.CountryIdFrom, COUNTRY_ID_FROM);
        await _command.ExecuteNonQueryAsync();
    }
}
