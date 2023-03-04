namespace PoundPupLegacy.Db.Writers;

internal sealed class SubdivisionWriter : DatabaseWriter<Subdivision>, IDatabaseWriter<Subdivision>
{
    private const string ID = "id";
    private const string NAME = "name";
    private const string COUNTRY_ID = "country_id";
    private const string SUBDIVISION_TYPE_ID = "subdivision_type_id";
    public static async Task<DatabaseWriter<Subdivision>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
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
                new ColumnDefinition{
                    Name = SUBDIVISION_TYPE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new SubdivisionWriter(command);

    }

    internal SubdivisionWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(Subdivision subdivision)
    {
        if (subdivision.Id is null)
            throw new NullReferenceException();
        try {
            WriteValue(subdivision.Id, ID);
            WriteValue(subdivision.Name.Trim(), NAME);
            WriteValue(subdivision.CountryId, COUNTRY_ID);
            WriteValue(subdivision.SubdivisionTypeId, SUBDIVISION_TYPE_ID);
            await _command.ExecuteNonQueryAsync();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.ToString());
        }
    }
}
