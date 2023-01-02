namespace PoundPupLegacy.Db.Writers;

internal class TopLevelCountryWriter : DatabaseWriter<TopLevelCountry>, IDatabaseWriter<TopLevelCountry>
{
    private const string ID = "id";
    private const string ISO_3166_1_CODE = "iso_3166_1_code";
    private const string GLOBAL_REGION_ID = "global_region_id";
    public static DatabaseWriter<TopLevelCountry> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "top_level_country",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ISO_3166_1_CODE,
                    NpgsqlDbType = NpgsqlDbType.Char
                },
                new ColumnDefinition{
                    Name = GLOBAL_REGION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );

        return new TopLevelCountryWriter(command);
    }

    internal TopLevelCountryWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(TopLevelCountry country)
    {
        if (country.Id is null)
            throw new NullReferenceException();
        WriteValue(country.Id, ID);
        WriteValue(country.ISO3166_1_Code, ISO_3166_1_CODE);
        WriteValue(country.GlobalRegionId, GLOBAL_REGION_ID);
        _command.ExecuteNonQuery();
    }
}
