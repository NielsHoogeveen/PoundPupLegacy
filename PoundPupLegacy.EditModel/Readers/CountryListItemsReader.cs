using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.EditModel.Readers;
public class CountryListItemsReaderFactory : IDatabaseReaderFactory<CountryListItemsReader>
{
    public async Task<CountryListItemsReader> CreateAsync(IDbConnection connection)
    {
        var sql = $"""
                select
                    c.id,
                    t.name
                    from country c
                    join term t on t.nameable_id = c.id
                    join tenant_node tn on tn.node_id = t.vocabulary_id
                    where tn.tenant_id = 1 and tn.url_id = 4126
            
                """;
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        command.CommandTimeout = 300;
        command.CommandText = sql;
        await command.PrepareAsync();
        return new CountryListItemsReader(command);

    }
}
public class CountryListItemsReader : DatabaseReader, IEnumerableDatabaseReader<CountryListItemsReader.Request, CountryListItem>
{
    public record Request { }
    internal CountryListItemsReader(NpgsqlCommand command) : base(command)
    {
    }
    public async IAsyncEnumerable<CountryListItem> ReadAsync(Request request)
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
