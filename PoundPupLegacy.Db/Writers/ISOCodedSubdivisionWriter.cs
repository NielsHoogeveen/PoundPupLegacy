namespace PoundPupLegacy.Db.Writers;

internal class ISOCodedSubdivisionWriter : DatabaseWriter<ISOCodedSubdivision>, IDatabaseWriter<ISOCodedSubdivision>
{
    private const string ID = "id";
    private const string ISO_3166_2_CODE = "iso_3166_2_code";
    public static DatabaseWriter<ISOCodedSubdivision> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "iso_coded_subdivision",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ISO_3166_2_CODE,
                    NpgsqlDbType = NpgsqlDbType.Char
                },
            }
        );
        return new ISOCodedSubdivisionWriter(command);
    }
    private ISOCodedSubdivisionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(ISOCodedSubdivision country)
    {
        WriteValue(country.Id, ID);
        WriteValue(country.ISO3166_2_Code, ISO_3166_2_CODE);
        _command.ExecuteNonQuery();
    }
}
