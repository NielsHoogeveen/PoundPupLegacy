using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using System.Data;

namespace PoundPupLegacy.Readers;
public class TenantsReaderFactory : IDatabaseReaderFactory<TenantsReader>
{
    public async Task<TenantsReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        await command.PrepareAsync();
        return new TenantsReader(command);
    }

    const string SQL = """
        select
        t.id,
        t.domain_name,
        t.country_id_default,
        n.title country_name
        from tenant t
        join node n on n.id = t.country_id_default
        """;

}
public class TenantsReader: EnumerableDatabaseReader<TenantsReader.TenantsRequest, Tenant>
{
    public record TenantsRequest
    {
    }

    internal TenantsReader(NpgsqlCommand command) : base(command)
    {
    }

    public override async IAsyncEnumerable<Tenant> ReadAsync(TenantsRequest request)
    {
        await using var reader = await _command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            var tenantId = reader.GetInt32(0);
            var domainName = reader.GetString(1);
            var countryIdDefault = reader.GetInt32(2);
            var countryNameDefault = reader.GetString(3);
            yield return new Tenant {
                Id = tenantId,
                DomainName = domainName,
                CountryIdDefault = countryIdDefault,
                CountryNameDefault = countryNameDefault,
                IdToUrl = new Dictionary<int, string>(),
                UrlToId = new Dictionary<string, int>()

            };
        }

    }


}
