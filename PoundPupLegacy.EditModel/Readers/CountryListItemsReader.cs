using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;
public class CountryListItemsReaderFactory : DatabaseReaderFactory<CountryListItemsReader>
{
    public override string Sql => SQL;
    private const string SQL = $"""
        select
            c.id,
            t.name
            from country c
            join term t on t.nameable_id = c.id
            join tenant_node tn on tn.node_id = t.vocabulary_id
            where tn.tenant_id = 1 and tn.url_id = 4126
            
        """;
}
public class CountryListItemsReader : EnumerableDatabaseReader<CountryListItemsReader.Request, CountryListItem>
{
    public record Request { }
    internal CountryListItemsReader(NpgsqlCommand command) : base(command)
    {
    }
    
    public override async IAsyncEnumerable<CountryListItem> ReadAsync(Request request)
    {
        await using var reader = await _command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            var id = reader.GetInt32(0);
            var name = reader.GetString(1);
            yield return new CountryListItem {
                Id = id,
                Name = name
            };
        }
    }
}
