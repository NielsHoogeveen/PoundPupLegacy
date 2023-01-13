namespace PoundPupLegacy.Db.Writers;

internal sealed class AdoptionExportYearWriter : DatabaseWriter<AdoptionExportYear>, IDatabaseWriter<AdoptionExportYear>
{

    private const string ADOPTION_EXPORT_RELATION_ID = "adoption_export_relation_id";
    private const string YEAR = "year";
    private const string NUMBER_OF_CHILDREN = "number_of_children";
    public static async Task<DatabaseWriter<AdoptionExportYear>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "adoption_export_year",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ADOPTION_EXPORT_RELATION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = YEAR,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NUMBER_OF_CHILDREN,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new AdoptionExportYearWriter(command);

    }

    internal AdoptionExportYearWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(AdoptionExportYear adoptionExportYear)
    {
        WriteValue(adoptionExportYear.AdoptionExportRelationId, ADOPTION_EXPORT_RELATION_ID);
        WriteValue(adoptionExportYear.Year, YEAR);
        WriteNullableValue(adoptionExportYear.NumberOfChildren, NUMBER_OF_CHILDREN);
        await _command.ExecuteNonQueryAsync();
    }
}
