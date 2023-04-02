namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SubdivisionInserter : DatabaseInserter<Subdivision>, IDatabaseInserter<Subdivision>
{
    private const string ID = "id";
    private const string NAME = "name";
    private const string COUNTRY_ID = "country_id";
    private const string SUBDIVISION_TYPE_ID = "subdivision_type_id";
    public static async Task<DatabaseInserter<Subdivision>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
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
        return new SubdivisionInserter(command);

    }

    internal SubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Subdivision subdivision)
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
