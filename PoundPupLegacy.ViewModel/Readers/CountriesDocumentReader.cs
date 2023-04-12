namespace PoundPupLegacy.ViewModel.Readers;

using Factory = CountriesDocumentReaderFactory;
using Reader = CountriesDocumentReader;

public class CountriesDocumentReaderFactory : DatabaseReaderFactory<Reader>
{
    internal readonly static NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };

    internal readonly static FieldValueReader<FirstLevelRegionListEntry[]> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        select
            jsonb_agg(
                jsonb_build_object(
                    'Name', 
                    n.title,
                    'Path', 
                    case
                        when n.url_path is null then '/node/' || n.url_id
                        else '/' || n.url_path
                    end,
                    'Regions', 
                    (
                        select 
                        	document 
                        from(
                        	select 
                        		jsonb_agg(
                                    jsonb_build_object(
                            			'Name', 
                                        n2.title,
                        				'Path', 
                                        case
                        					when n2.url_path is null then '/node/' || n2.url_id
                        					else '/' || n2.url_path
                        				end,
                        				'Countries', 
                                        (
                            				select 
                            					document 
                            				from(
                            					select 
                            						jsonb_agg(
                                                        jsonb_build_object(
                        								    'Name', 
                                                            n3.title,
                        								    'Path', 
                                                            case
                        										when n3.url_path is null then '/node/' || n3.url_id
                        										else '/' || n3.url_path
                        									end
                        							    )
                                                    ) document
                        						FROM (select * from node n3
                        						join tenant_node tn3 on tn3.node_id = n3.id and tn3.tenant_id = 1
                        						JOIN top_level_country tlc2 on tlc2.global_region_id = n2.id and tlc2.id = n3.id
                                                ORDER BY n3.title
            								) n3
                        				) x
                        			) 
                        	)) document
                        	FROM (
            				select 
            					n2.id,
            					tn2.url_id,
            					tn2.url_path,
            					n2.title
            					from 
            				node n2
                        	join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = 1
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
                        			'Name', n2.title,
                        			'Path', case
                        						when n2.url_path is null then '/node/' || n2.url_id
                        						else '/' || n2.url_path
                        					end
                        		    )
                                ) document
                        	FROM (
            					select
            					n2.id,
            					n2.title,
            					tn2.url_id,
            					tn2.url_path
            					from node n2
            					join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = 1
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
            	tn.url_id,
            	tn.url_path
            from node n
            join first_level_global_region r on r.id = n.id
            join tenant_node tn on tn.node_id = n.id and tn.tenant_id = 1
            ORDER BY n.title
        ) n
        """;
}
public class CountriesDocumentReader : SingleItemDatabaseReader<int, FirstLevelRegionListEntry[]>
{
    public CountriesDocumentReader(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(int request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.TenantIdParameter, request)
        };
    }

    protected override FirstLevelRegionListEntry[] Read(NpgsqlDataReader reader)
    {
        return Factory.DocumentReader.GetValue(reader);

    }
}