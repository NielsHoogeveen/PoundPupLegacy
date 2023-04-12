using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;
public class SubdivisionListItemsReaderFactory : DatabaseReaderFactory<SubdivisionListItemsReader>
{
    internal static NonNullableIntegerDatabaseParameter CountryId = new() { Name = "country_id" };
    public override string Sql => SQL;
    private const string SQL = $"""
        select
            s.id,
            s.name
            from subdivision s
            join bottom_level_subdivision bls on bls.id = s.id
            where s.country_id = @country_id
            order by s.name
        """;
}
public class SubdivisionListItemsReader : EnumerableDatabaseReader<int, SubdivisionListItem>
{
    internal SubdivisionListItemsReader(NpgsqlCommand command) : base(command)
    {
    }
    public override async IAsyncEnumerable<SubdivisionListItem> ReadAsync(int countryId)
    {
        _command.Parameters["country_id"].Value = countryId;
        await using var reader = await _command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);
            yield return new SubdivisionListItem {
                Id = id,
                Name = name
            };
        }
    }
}
