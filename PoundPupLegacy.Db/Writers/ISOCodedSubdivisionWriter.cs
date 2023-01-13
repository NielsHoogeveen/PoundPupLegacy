namespace PoundPupLegacy.Db.Writers;

internal sealed class ISOCodedSubdivisionWriter : DatabaseWriter<ISOCodedSubdivision>, IDatabaseWriter<ISOCodedSubdivision>
{
    private const string ID = "id";
    private const string ISO_3166_2_CODE = "iso_3166_2_code";
    public static async Task<DatabaseWriter<ISOCodedSubdivision>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
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

    internal override async Task WriteAsync(ISOCodedSubdivision country)
    {
        if (country.Id is null)
            throw new NullReferenceException();

        WriteValue(country.Id, ID);
        WriteValue(country.ISO3166_2_Code, ISO_3166_2_CODE);
        await _command.ExecuteScalarAsync();
    }
}
