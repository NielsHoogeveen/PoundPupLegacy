using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Factory = TenantsReaderFactory;
using Reader = TenantsReader;
internal sealed class TenantsReaderFactory : DatabaseReaderFactory<Reader>
{
    internal static readonly IntValueReader TenantIdReader = new() { Name = "tenant_id" };
    internal static readonly StringValueReader DomainNameReader = new() { Name = "domain_name" };
    internal static readonly IntValueReader CountryIdDefaultReader = new() { Name = "country_id_default" };
    internal static readonly StringValueReader CountryNameDefault = new() { Name = "country_name" };


    public override string Sql => SQL;

    const string SQL = """
        select
        t.id tenant_id,
        t.domain_name,
        t.country_id_default,
        n.title country_name
        from tenant t
        join node n on n.id = t.country_id_default
        """;

}
internal sealed class TenantsReader : EnumerableDatabaseReader<Reader.Request, Tenant>
{
    public record Request
    {
    }

    public TenantsReader(NpgsqlCommand command) : base(command)
    {
    }
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] { };
    }

    protected override Tenant Read(NpgsqlDataReader reader)
    {
        return new Tenant {
            Id = Factory.TenantIdReader.GetValue(reader),
            DomainName = Factory.DomainNameReader.GetValue(reader),
            CountryIdDefault = Factory.CountryIdDefaultReader.GetValue(reader),
            CountryNameDefault = Factory.CountryNameDefault.GetValue(reader),
            IdToUrl = new Dictionary<int, string>(),
            UrlToId = new Dictionary<string, int>()
        };
    }
}
