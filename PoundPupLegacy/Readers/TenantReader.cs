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

    public override string Sql => SQL;

    const string SQL = """
        select
        jsonb_build_object(
            'Id',
            t.id,
            'Name',
            ug.name,
            'Description',
            ug.description,
            'DomainName',
            t.domain_name,
            'CountryIdDefault',
            t.country_id_default,
            'CountryNameDefault',
            n.title,
            'FrontPageText',
            t.front_page_text,
            'FrontPageId',
            t.frontpage_id,
            'Logo',
            t.logo,
            'Subtitle',
            t.sub_title,
            'FooterText',
            t.footer_text,
            'CssFile',
            t.css_file,
            'Title',
            t.title,
            'GoogleAnalyticsMeasurementId',
            t.google_analytics_measurement_id,
            'RegistrationText',
            t.registration_text,
            'TrackActiveUsers',
            t.track_active_users,
            'Subgroups',
            (
                select
                jsonb_agg(
        	        jsonb_build_object(
                        'Id',
                        x.id,
        		        'Name',
        		        name,
        		        'Path',
        		        path,
        		        'Description',
        		        description
        	        )	
        	        order by name
                ) document
                from(
        	        select
                    ug.id,
        	        ug.name,
        	        ug.description,
        	        '/group/' || sg.id path
        	        from subgroup sg
        	        join user_group ug on ug.id = sg.id
        	        where tenant_id = @tenant_id
                ) x
             ),
            'SmtpConnection',
            jsonb_build_object(
                'Id',
                sc.id,
                'Host',
                sc.host,
                'Port',
                sc.port,
                'UserName',
                sc.user_name,
                'Password',
                sc.password
            ),
            'TenantNodes',
            (
                select jsonb_agg(
                    jsonb_build_object(
                        'TenantId',
                        tn.tenant_id,
                        'UrlPath',
                        tn.url_path,
                        'UrlId',
                        tn.url_id
                    )
                )
                from tenant_node tn
                where tn.tenant_id = t.id
                and tn.url_path is not null
            )
        ) as document
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
        var tenant = reader.GetFieldValue<Tenant>(0);
        return tenant;
    }

    protected override string GetErrorMessage(Request request)
    {
        return $"No tenant was found with Id {request.TenantId}";
    }
}
