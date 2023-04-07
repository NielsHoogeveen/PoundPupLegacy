namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ISOCodedSubdivisionInserterFactory : DatabaseInserterFactory<ISOCodedSubdivision>
{
    public override async Task<IDatabaseInserter<ISOCodedSubdivision>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "iso_coded_subdivision",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ISOCodedSubdivisionInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ISOCodedSubdivisionInserter.ISO_3166_2_CODE,
                    NpgsqlDbType = NpgsqlDbType.Char
                },
            }
        );
        return new ISOCodedSubdivisionInserter(command);
    }
}
internal sealed class ISOCodedSubdivisionInserter : DatabaseInserter<ISOCodedSubdivision>
{
    internal const string ID = "id";
    internal const string ISO_3166_2_CODE = "iso_3166_2_code";
    internal ISOCodedSubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(ISOCodedSubdivision country)
    {
        if (country.Id is null)
            throw new NullReferenceException();

        SetParameter(country.Id, ID);
        SetParameter(country.ISO3166_2_Code, ISO_3166_2_CODE);
        await _command.ExecuteScalarAsync();
    }
}
