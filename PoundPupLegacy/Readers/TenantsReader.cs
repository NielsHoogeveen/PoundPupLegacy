using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Request = TenantsReaderRequest;

internal sealed record TenantsReaderRequest : IRequest
{

}

internal sealed class TenantsReaderFactory : EnumerableDatabaseReaderFactory<Request, Tenant>
{
    private static readonly IntValueReader TenantIdReader = new() { Name = "tenant_id" };
    private static readonly StringValueReader DomainNameReader = new() { Name = "domain_name" };
    private static readonly IntValueReader CountryIdDefaultReader = new() { Name = "country_id_default" };
    private static readonly StringValueReader CountryNameDefault = new() { Name = "country_name" };
    private static readonly NullableStringValueReader FrontPageText = new() { Name = "front_page_text" };

    public override string Sql => SQL;

    const string SQL = """
        select
        t.id tenant_id,
        t.domain_name,
        t.country_id_default,
        n.title country_name,
        t.front_page_text
        from tenant t
        join node n on n.id = t.country_id_default
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] { };
    }

    protected override Tenant Read(NpgsqlDataReader reader)
    {
        return new Tenant {
            Id = TenantIdReader.GetValue(reader),
            DomainName = DomainNameReader.GetValue(reader),
            CountryIdDefault = CountryIdDefaultReader.GetValue(reader),
            CountryNameDefault = CountryNameDefault.GetValue(reader),
            IdToUrl = new Dictionary<int, string>(),
            UrlToId = new Dictionary<string, int>(),
            FrontPageText = FrontPageText.GetValue(reader),
        };
    }
}
