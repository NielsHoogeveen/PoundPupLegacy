using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Request = TenantReaderRequest;

public sealed record TenantReaderRequest : IRequest
{
    public required int TenantId { get; init; }
}

internal sealed class TenantReaderFactory : MandatorySingleItemDatabaseReaderFactory<Request, Tenant>
{

    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };

    private static readonly IntValueReader TenantIdReader = new() { Name = "tenant_id" };
    private static readonly StringValueReader NameReader = new() { Name = "name" };
    private static readonly StringValueReader TitleReader = new() { Name = "title" };
    private static readonly StringValueReader DescriptionReader = new() { Name = "description" };
    private static readonly StringValueReader DomainNameReader = new() { Name = "domain_name" };
    private static readonly IntValueReader CountryIdDefaultReader = new() { Name = "country_id_default" };
    private static readonly StringValueReader CountryNameDefault = new() { Name = "country_name" };
    private static readonly NullableStringValueReader FrontPageText = new() { Name = "front_page_text" };
    private static readonly NullableStringValueReader Logo = new() { Name = "logo" };
    private static readonly NullableStringValueReader CssFile = new() { Name = "css_file" };
    private static readonly NullableStringValueReader SubTitle = new() { Name = "sub_title" };
    private static readonly NullableStringValueReader FooterText = new() { Name = "footer_text" };
    private static readonly NullableStringValueReader GoogleAnalyticsMeasurementId = new() { Name = "google_analytics_measurement_id" };
    private static readonly IntValueReader FrontPageId = new() { Name = "frontpage_id" };
    private static readonly IntValueReader SmtpConnectionId = new() { Name = "smtp_connection_id" };
    private static readonly StringValueReader SmptHost = new() { Name = "smtp_host" };
    private static readonly IntValueReader SmtpPort = new() { Name = "smtp_port" };
    private static readonly StringValueReader SmptUserName = new() { Name = "smtp_user_name" };
    private static readonly StringValueReader SmptPassword = new() { Name = "smtp_password" };
    private static readonly NullableStringValueReader RegistrationText = new() { Name = "registration_text" };

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
        t.frontpage_id,
        t.logo,
        t.sub_title,
        t.footer_text,
        t.css_file,
        t.title,
        t.google_analytics_measurement_id,
        t.registration_text,
        sc.id smtp_connection_id,
        sc.host smtp_host,
        sc.port smtp_port,
        sc.user_name  smtp_user_name,
        sc.password  smtp_password
        from tenant t
        join user_group ug on ug.id = t.id
        join node n on n.id = t.country_id_default
        join smtp_connection sc on sc.id = t.smtp_connection_id
        where t.id = @tenant_id
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
        };
    }

    protected override Tenant Read(NpgsqlDataReader reader)
    {
        return new Tenant {
            Id = TenantIdReader.GetValue(reader),
            Name = NameReader.GetValue(reader),
            Title = TitleReader.GetValue(reader),
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
            FrontPageId = FrontPageId.GetValue(reader),
            GoogleAnalyticsMeasurementId = GoogleAnalyticsMeasurementId.GetValue(reader),
            RegistrationText = RegistrationText.GetValue(reader),
            SmtpConnection = new SmtpConnection { 
                Id = SmtpConnectionId.GetValue(reader),
                Host = SmptHost.GetValue(reader),
                Port = SmtpPort.GetValue(reader),
                Username = SmptUserName.GetValue(reader),
                Password = SmptPassword.GetValue(reader)
            }
        };
    }

    protected override string GetErrorMessage(Request request)
    {
        return $"No tenant was found with Id {request.TenantId}";
    }
}
