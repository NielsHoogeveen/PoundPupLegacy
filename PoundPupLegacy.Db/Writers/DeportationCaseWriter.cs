namespace PoundPupLegacy.Db.Writers;

internal class DeportationCaseWriter : DatabaseWriter<DeportationCase>, IDatabaseWriter<DeportationCase>
{
    private const string ID = "id";
    private const string SUBDIVISION_ID_FROM = "subdivision_id_from";
    private const string COUNTRY_ID_TO = "country_id_to";
    public static DatabaseWriter<DeportationCase> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
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

    internal override void Write(DeportationCase deportationCase)
    {
        if (deportationCase.Id is null)
            throw new NullReferenceException();

        WriteValue(deportationCase.Id, ID);
        WriteNullableValue(deportationCase.SubdivisionIdFrom, SUBDIVISION_ID_FROM);
        WriteNullableValue(deportationCase.CountryIdTo, COUNTRY_ID_TO);
        _command.ExecuteNonQuery();
    }
}
