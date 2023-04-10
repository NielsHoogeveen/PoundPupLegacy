namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class SubdivisionInserterFactory : DatabaseInserterFactory<Subdivision>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionTypeId = new() { Name = "subdivision_type_id" };

    public override async Task<IDatabaseInserter<Subdivision>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "subdivision",
            new DatabaseParameter[] {
                Id,
                Name,
                CountryId,
                SubdivisionTypeId
            }
        );
        return new SubdivisionInserter(command);
    }
}
internal sealed class SubdivisionInserter : DatabaseInserter<Subdivision>
{
    internal SubdivisionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Subdivision subdivision)
    {
        if (subdivision.Id is null)
            throw new NullReferenceException();
        try {
            Set(SubdivisionInserterFactory.Id, subdivision.Id.Value);
            Set(SubdivisionInserterFactory.Name, subdivision.Name.Trim());
            Set(SubdivisionInserterFactory.CountryId, subdivision.CountryId);
            Set(SubdivisionInserterFactory.SubdivisionTypeId, subdivision.SubdivisionTypeId);
            await _command.ExecuteNonQueryAsync();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.ToString());
        }
    }
}
