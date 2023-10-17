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
    private static readonly StringValueReader NameReader = new() { Name = "name" };
    private static readonly StringValueReader DescriptionReader = new() { Name = "description" };
    private static readonly StringValueReader DomainNameReader = new() { Name = "domain_name" };
    private static readonly IntValueReader CountryIdDefaultReader = new() { Name = "country_id_default" };
    private static readonly StringValueReader CountryNameDefault = new() { Name = "country_name" };
    private static readonly NullableStringValueReader FrontPageText = new() { Name = "front_page_text" };
    private static readonly NullableStringValueReader Logo = new() { Name = "logo" };
    private static readonly NullableStringValueReader CssFile = new() { Name = "css_file" };
    private static readonly NullableStringValueReader SubTitle = new() { Name = "sub_title" };
    private static readonly NullableStringValueReader FooterText = new() { Name = "footer_text" };

    public override string Sql => SQL;

    const string SQL = """
        select
        t.id tenant_id,
        ug.name,
        ug.description,
        t.domain_name,
        t.country_id_default,
        n.title country_name,
        t.front_page_text,
        t.logo,
        t.sub_title,
        t.footer_text,
        t.css_file
        from tenant t
        join user_group ug on ug.id = t.id
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
            Name = NameReader.GetValue(reader),
            Description = DescriptionReader.GetValue(reader),
            DomainName = DomainNameReader.GetValue(reader),
            CountryIdDefault = CountryIdDefaultReader.GetValue(reader),
            CountryNameDefault = CountryNameDefault.GetValue(reader),
            IdToUrl = new Dictionary<int, string>(),
            UrlToId = new Dictionary<string, int>(),
            FrontPageText = FrontPageText.GetValue(reader),
            Logo = Logo.GetValue(reader),
            CssFile = CssFile.GetValue(reader),
            Subtitle = SubTitle.GetValue(reader),
            FooterText = FooterText.GetValue(reader),
        };
    }
}
