namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ISOCodedSubdivisionInserter : DatabaseInserter<ISOCodedSubdivision>, IDatabaseInserter<ISOCodedSubdivision>
{
    private const string ID = "id";
    private const string ISO_3166_2_CODE = "iso_3166_2_code";
    public static async Task<DatabaseInserter<ISOCodedSubdivision>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
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
        return new ISOCodedSubdivisionInserter(command);
    }
    private ISOCodedSubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(ISOCodedSubdivision country)
    {
        if (country.Id is null)
            throw new NullReferenceException();

        WriteValue(country.Id, ID);
        WriteValue(country.ISO3166_2_Code, ISO_3166_2_CODE);
        await _command.ExecuteScalarAsync();
    }
}
