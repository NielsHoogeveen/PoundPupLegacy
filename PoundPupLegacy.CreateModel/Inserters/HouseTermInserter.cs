namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class HouseTermInserterFactory : DatabaseInserterFactory<HouseTerm>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter RepresentativeId = new() { Name = "representative_id" };
    internal static NonNullableIntegerDatabaseParameter SubdivisionId = new() { Name = "subdivision_id" };
    internal static NullableIntegerDatabaseParameter District = new() { Name = "district" };
    internal static NonNullableDateRangeDatabaseParameter DateRange = new() { Name = "date_range" };

    public override async Task<IDatabaseInserter<HouseTerm>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "house_term",
            new DatabaseParameter[] {
                Id,
                RepresentativeId,
                SubdivisionId,
                District,
                DateRange
            }
        );
        return new HouseTermInserter(command);

    }

}
internal sealed class HouseTermInserter : DatabaseInserter<HouseTerm>
{

    internal HouseTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(HouseTerm houseTerm)
    {
        if (houseTerm.Id is null)
            throw new NullReferenceException();
        if (houseTerm.RepresentativeId is null)
            throw new NullReferenceException();
        Set(HouseTermInserterFactory.Id, houseTerm.Id.Value);
        Set(HouseTermInserterFactory.RepresentativeId, houseTerm.RepresentativeId.Value);
        Set(HouseTermInserterFactory.SubdivisionId, houseTerm.SubdivisionId);
        Set(HouseTermInserterFactory.District, houseTerm.District);
        Set(HouseTermInserterFactory.DateRange, houseTerm.DateTimeRange);
        await _command.ExecuteNonQueryAsync();
    }
}
