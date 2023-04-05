namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class DeportationCaseInserterFactory : DatabaseInserterFactory<DeportationCase>
{
    public override async Task<IDatabaseInserter<DeportationCase>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "deportation_case",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = DeportationCaseInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DeportationCaseInserter.SUBDIVISION_ID_FROM,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DeportationCaseInserter.COUNTRY_ID_TO,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new DeportationCaseInserter(command);
    }
}

internal sealed class DeportationCaseInserter : DatabaseInserter<DeportationCase>
{
    internal const string ID = "id";
    internal const string SUBDIVISION_ID_FROM = "subdivision_id_from";
    internal const string COUNTRY_ID_TO = "country_id_to";

    internal DeportationCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(DeportationCase deportationCase)
    {
        if (deportationCase.Id is null)
            throw new NullReferenceException();

        WriteValue(deportationCase.Id, ID);
        WriteNullableValue(deportationCase.SubdivisionIdFrom, SUBDIVISION_ID_FROM);
        WriteNullableValue(deportationCase.CountryIdTo, COUNTRY_ID_TO);
        await _command.ExecuteNonQueryAsync();
    }
}
