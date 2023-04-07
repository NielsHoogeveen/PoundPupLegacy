namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class HouseTermInserterFactory : DatabaseInserterFactory<HouseTerm>
{
    public override async Task<IDatabaseInserter<HouseTerm>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "house_term",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = HouseTermInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = HouseTermInserter.REPRESENTATIVE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = HouseTermInserter.SUBDIVISION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = HouseTermInserter.DISTRICT,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = HouseTermInserter.DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new HouseTermInserter(command);

    }

}
internal sealed class HouseTermInserter : DatabaseInserter<HouseTerm>
{
    internal const string ID = "id";
    internal const string REPRESENTATIVE_ID = "representative_id";
    internal const string SUBDIVISION_ID = "subdivision_id";
    internal const string DISTRICT = "district";
    internal const string DATE_RANGE = "date_range";

    internal HouseTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(HouseTerm houseTerm)
    {
        if (houseTerm.Id is null)
            throw new NullReferenceException();
        if (houseTerm.RepresentativeId is null)
            throw new NullReferenceException();
        SetParameter(houseTerm.Id, ID);
        SetParameter(houseTerm.RepresentativeId, REPRESENTATIVE_ID);
        SetParameter(houseTerm.SubdivisionId, SUBDIVISION_ID);
        SetNullableParameter(houseTerm.District, DISTRICT);
        SetDateTimeRangeParameter(houseTerm.DateTimeRange, DATE_RANGE);
        await _command.ExecuteNonQueryAsync();
    }
}
