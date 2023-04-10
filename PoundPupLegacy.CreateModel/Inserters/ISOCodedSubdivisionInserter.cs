namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class ISOCodedSubdivisionInserterFactory : DatabaseInserterFactory<ISOCodedSubdivision>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableFixedStringDatabaseParameter ISO31661_2_Code = new() { Name = "iso_3166_2_code" };

    public override async Task<IDatabaseInserter<ISOCodedSubdivision>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "iso_coded_subdivision",
            new DatabaseParameter[] {
                Id,
                ISO31661_2_Code
            }
        );
        return new ISOCodedSubdivisionInserter(command);
    }
}
internal sealed class ISOCodedSubdivisionInserter : DatabaseInserter<ISOCodedSubdivision>
{
    internal ISOCodedSubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(ISOCodedSubdivision country)
    {
        if (country.Id is null)
            throw new NullReferenceException();

        Set(ISOCodedSubdivisionInserterFactory.Id, country.Id.Value);
        Set(ISOCodedSubdivisionInserterFactory.ISO31661_2_Code, country.ISO3166_2_Code);
        await _command.ExecuteScalarAsync();
    }
}
