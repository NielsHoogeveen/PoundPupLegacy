namespace PoundPupLegacy.Db.Writers;

internal sealed class DeportationCaseWriter : DatabaseWriter<DeportationCase>, IDatabaseWriter<DeportationCase>
{
    private const string ID = "id";
    private const string SUBDIVISION_ID_FROM = "subdivision_id_from";
    private const string COUNTRY_ID_TO = "country_id_to";
    public static async Task<DatabaseWriter<DeportationCase>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "deportation_case",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SUBDIVISION_ID_FROM,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = COUNTRY_ID_TO,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new DeportationCaseWriter(command);

    }

    internal DeportationCaseWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(DeportationCase deportationCase)
    {
        if (deportationCase.Id is null)
            throw new NullReferenceException();

        WriteValue(deportationCase.Id, ID);
        WriteNullableValue(deportationCase.SubdivisionIdFrom, SUBDIVISION_ID_FROM);
        WriteNullableValue(deportationCase.CountryIdTo, COUNTRY_ID_TO);
        await _command.ExecuteNonQueryAsync();
    }
}
