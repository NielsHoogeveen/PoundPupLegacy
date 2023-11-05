namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = CountriesDocumentReaderRequest;

public sealed class CountriesDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
}

internal sealed class CountriesDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, FirstLevelRegionListEntry[]>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };

    private static readonly FieldValueReader<FirstLevelRegionListEntry[]> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        with
        {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS}
        select
            jsonb_agg(
                jsonb_build_object(
                    'Title', 
                    n.title,
                    'Path', 
                    '/' || n.viewer_path || '/' || n.id,
                    'Regions', 
                    (
                        select 
                        	document 
                            from
                            (
                        	    select 
                        		jsonb_agg(
                                    jsonb_build_object(
                            			'Title', 
                                        n2.title,
                        				'Path', 
                                        '/' || n2.viewer_path || '/'  || n2.id,
                        				'Countries', 
                                        (
                            				select 
                            					document 
                            				from
                                            (
                            					select 
                            						jsonb_agg(
                                                        jsonb_build_object(
                        								    'Title', 
                                                            n3.title,
                        								    'Path', 
                                                            '/' || n3.viewer_path ||'/' || n3.node_id
                        							    )
                                                    ) document
                        						FROM (
                                                    select * 
                                                    from node n3
                                                    join node_type nt3 on n3.node_type_id = nt3.id
                        						    join tenant_node tn3 on tn3.node_id = n3.id and tn3.tenant_id = @tenant_id
                        						    JOIN top_level_country tlc2 on tlc2.global_region_id = n2.id and tlc2.id = n3.id
                                                    ORDER BY n3.title
            								    ) n3
                        				    ) x
                        			    ) 
                        	        )
                                ) document
                        	FROM (
            				    select 
            					n2.id,
            					n2.title,
                                nt2.viewer_path
            				    from node n2
                                join node_type nt2 on n2.node_type_id = nt2.id
                        	    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
                        	    JOIN second_level_global_region r2 on r2.first_level_global_region_id = n.id and r2.id = n2.id
                                ORDER BY n2.title
            				) n2
                        ) x
                    ),
                    'Countries', 
                    (
                        select 
                        	document 
                        from(
                        	select 
                        		jsonb_agg(
                                    jsonb_build_object(
                        			    'Title', 
                                        n2.title,
                        			    'Path', 
                                        '/' || n2.viewer_path || '/' || n2.id
                        		    )
                                ) document
                        	FROM (
            					select
            					n2.id,
            					n2.title,
                                nt2.viewer_path
            					from node n2
                                join node_type nt2 on n2.node_type_id = nt2.id
            					join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
            					JOIN top_level_country tlc on tlc.global_region_id = n.id and tlc.id = n2.id
            					ORDER BY n2.title
            				) n2
                        ) x
                    )
                )
            ) document
        from (
            select 
            	n.id,
            	n.title,
                nt.viewer_path
            from node n
            join node_type nt on n.node_type_id = nt.id
            join first_level_global_region r on r.id = n.id
            join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id
            ORDER BY n.title
        ) n
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId)
        };
    }

    protected override FirstLevelRegionListEntry[] Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);

    }
}