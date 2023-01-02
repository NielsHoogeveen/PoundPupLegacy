namespace PoundPupLegacy.Db.Writers;

internal class SubdivisionWriter : DatabaseWriter<Subdivision>, IDatabaseWriter<Subdivision>
{
    private const string ID = "id";
    private const string NAME = "name";
    private const string COUNTRY_ID = "country_id";
    public static DatabaseWriter<Subdivision> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "subdivision",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NAME,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = COUNTRY_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new SubdivisionWriter(command);

    }

    internal SubdivisionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Subdivision subdivision)
    {
        if (subdivision.Id is null)
            throw new NullReferenceException();

        WriteValue(subdivision.Id, ID);
        WriteValue(subdivision.Name, NAME);
        WriteValue(subdivision.CountryId, COUNTRY_ID);
        _command.ExecuteNonQuery();
    }
}
