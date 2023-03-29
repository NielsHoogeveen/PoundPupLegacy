namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CountrySubdivisionTypeWriter : DatabaseInserter<CountrySubdivisionType>, IDatabaseInserter<CountrySubdivisionType>
{

    private const string COUNTRY_ID = "country_id";
    private const string SUBDIVISION_TYPE_ID = "subdivision_type_id";
    public static async Task<DatabaseInserter<CountrySubdivisionType>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "country_subdivision_type",
            new ColumnDefinition[] {
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
        return new CountrySubdivisionTypeWriter(command);

    }

    internal CountrySubdivisionTypeWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(CountrySubdivisionType countrySubdivisionType)
    {
        WriteValue(countrySubdivisionType.CountryId, COUNTRY_ID);
        WriteValue(countrySubdivisionType.SubdivisionTypeId, SUBDIVISION_TYPE_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
