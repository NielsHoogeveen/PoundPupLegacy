namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class HouseTermInserter : DatabaseInserter<HouseTerm>, IDatabaseInserter<HouseTerm>
{
    private const string ID = "id";
    private const string REPRESENTATIVE_ID = "representative_id";
    private const string SUBDIVISION_ID = "subdivision_id";
    private const string DISTRICT = "district";
    private const string DATE_RANGE = "date_range";
    public static async Task<DatabaseInserter<HouseTerm>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "house_term",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = REPRESENTATIVE_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SUBDIVISION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DISTRICT,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new HouseTermInserter(command);

    }

    internal HouseTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(HouseTerm houseTerm)
    {
        if (houseTerm.Id is null)
            throw new NullReferenceException();
        if (houseTerm.RepresentativeId is null)
            throw new NullReferenceException();
        WriteValue(houseTerm.Id, ID);
        WriteValue(houseTerm.RepresentativeId, REPRESENTATIVE_ID);
        WriteValue(houseTerm.SubdivisionId, SUBDIVISION_ID);
        WriteNullableValue(houseTerm.District, DISTRICT);
        WriteDateTimeRange(houseTerm.DateTimeRange, DATE_RANGE);
        await _command.ExecuteNonQueryAsync();
    }
}
