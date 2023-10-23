namespace PoundPupLegacy.ViewModel.Readers;
using PoundPupLegacy.ViewModel.Models;
using Request = DocumentsDocumentReaderRequest;

public sealed record DocumentsDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int[] SelectedTerms { get; init; }
    public required int StartIndex { get; init; }
    public required int Length { get; init; }
}

internal sealed class DocumentsDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, Documents>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NullableIntegerDatabaseParameter LengthParameter = new() { Name = "length" };
    private static readonly NullableIntegerDatabaseParameter StartIndexParameter = new() { Name = "start_index" };
    private static readonly NullableIntegerArrayDatabaseParameter TermsParameter = new() { Name = "terms" };

    private static readonly FieldValueReader<Documents> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        with
        {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS}
        select
            jsonb_build_object(
                'TermNames',
                terms,
                'Items',
                documents
            ) document
        from(
            select
            jsonb_agg(
        	    jsonb_build_object(
        		    'Id',
        		    id,
        		    'Name',
        		    title,
                    'Selected',
                    selected
        	    )
            ) terms,
            (
        	    select
        	    jsonb_build_object(
        		    'NumberOfEntries',
        		    number_of_elements,
        		    'Entries',
        		    elements
        	    ) documents
        	    from(
        		    select
        		    number_of_elements,
        		    jsonb_agg(
        			    jsonb_build_object(
        				    'Path',
        				    path,
        				    'Title',
        				    title,
        				    'Text',
        				    teaser,
        				    'HasBeenPublished',
        				    has_been_published,
                            'PublicationStatusId',
                            publication_status_id,
        				    'Weight',
        				    weight,
                            'Tags',
                            terms
        			    )
        		    ) elements
        		    from(
        			    select
        				    node_id,
                            number_of_elements,
        				    title,
        				    teaser,
        				    path,
                            changed_date_time,
        				    case 
        					    when publication_status_id = 0 then false
        					    else true
        				    end has_been_published,
                            publication_status_id,
        				    sum(weight) weight,
                            jsonb_agg(
                                jsonb_build_object(
                                    'Path',
                                    term_path,
                                    'Title',
                                    term_name,
                                    'NodeTypeName',
                                    term_type_name
                                )
                            ) terms
        			    from(
        				    select 
                            count(*) over() number_of_elements,
        				    t.id term_id,
        				    nt.node_id,
        				    n.title,
                            n.changed_date_time,
                            tn.publication_status_id,
                            lower(d.published) publication_date,
        				    stn.teaser,
        				    case
        					    when tn.url_path is null then '/node/' || tn.url_id
        					    else '/' || tn.url_path
        				    end path,
                            t.name term_name,
                            case 
                                when tn2.url_path is null then '/node/' || tn2.url_id
                                else '/' || tn2.url_path
                            end term_path,
                            nt2.tag_label_name term_type_name,
        				    case 
        					    when n.node_type_id = 41 then 5
        					    when n.node_type_id = 23 then 2
        					    else 1
        				    end weight
        				    from node n
                            join simple_text_node stn on stn.id = n.id
                            join document d on d.id = n.id
        				    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id
        				    join node_term nt on nt.node_id = n.id 
        				    join term t on t.id = nt.term_id
                            join node n2 on n2.id = t.nameable_id
                            left join nameable_type nt2 on nt2.id = n2.node_type_id and nt2.tag_label_name is not null
                            join tenant_node tn2 on tn2.node_id = t.nameable_id and tn2.tenant_id = @tenant_id
        				    where tn.publication_status_id in 
                            (
                                select 
                                id 
                                from accessible_publication_status 
                                where tenant_id = tn.tenant_id 
                                and (
                                    subgroup_id = tn.subgroup_id 
                                    or subgroup_id is null and tn.subgroup_id is null
                                )
                            )
                            and tn2.publication_status_id in 
                            (
                                select 
                                id 
                                from accessible_publication_status 
                                where tenant_id = tn2.tenant_id 
                                and (
                                    subgroup_id = tn2.subgroup_id 
                                    or subgroup_id is null and tn2.subgroup_id is null
                                )
                            )
                            and (@terms is null or n.id in (
                                select
                                node_id
                                from (
                                    select
                                    nt.node_id,
                                    count(*) over() c
                                    from term t
                                    left join node_term nt on nt.term_id = t.id and nt.node_id = n.id
                                    where t.id = ANY(@terms)
                                ) x
                                group by node_id, c
                                having count(node_id) = c
                            ))
                            and n.node_type_id = 10
                            and stn.teaser <> ''
        			    ) x
        			    GROUP BY 
        				    node_id,
        				    title,
        				    path,
        				    teaser,
        				    has_been_published,
                            publication_status_id,
                            number_of_elements,
                            changed_date_time,
                            publication_date
        			    ORDER BY publication_date desc nulls last
                        LIMIT @length OFFSET @start_index
        		    ) x
                    GROUP BY number_of_elements
        	    ) x
            ) documents
            from(
        	    select
        	    id,
        	    title,
                selected,
        	    sum(weight) weight
        	    from(
        		    select
        		    x.id,
        		    x.title,
        		    x.weight,
                    CASE
                        WHEN x.id = any(@terms) THEN true
                        ELSE false
                    END selected
                    from(
        			    select
        				    t.id,
        				    n.title,
        				    case 
        					    when n.node_type_id = 41 then 5
        					    when n.node_type_id = 23 then 2
        					    else 1
        				    end weight
        			    from node n
        			    join term t on t.nameable_id = n.id 
        			    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id
        			    join vocabulary v on v.id = t.vocabulary_id
        			    join node_term nt on nt.term_id = t.id
        			    join node n2 on n2.id = nt.node_id
                        join simple_text_node stn on stn.id = n2.id
        			    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
        			    where tn.publication_status_id in 
                        (
                            select 
                            id 
                            from accessible_publication_status 
                            where tenant_id = tn.tenant_id 
                            and (
                                subgroup_id = tn.subgroup_id 
                                or subgroup_id is null and tn.subgroup_id is null
                            )
                        )
                        and tn2.publication_status_id in 
                        (
                            select 
                            id 
                            from accessible_publication_status 
                            where tenant_id = tn2.tenant_id 
                            and (
                                subgroup_id = tn2.subgroup_id 
                                or subgroup_id is null and tn2.subgroup_id is null
                            )
                        )
                        and v.name = 'Topics'
        			    and (@terms is null or n2.id in (
                            select
                            node_id
                            from (
                                select
                                nt.node_id,
                                count(*) over() c
                                from term t
                                left join node_term nt on nt.term_id = t.id and nt.node_id = n2.id
                                where t.id = ANY(@terms)
                            ) x
                            group by node_id, c
                            having count(node_id) = c
                        ))
                        and n2.node_type_id = 10
                        and stn.teaser <> ''
        		    ) x
        	    )x
        	    group by 
        	    x.id,
        	    x.title,
                x.selected
        	    order by sum(x.weight) desc
        	    LIMIT 25 OFFSET 0
            ) x
        ) x
        """;


    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
            ParameterValue.Create(LengthParameter, request.Length),
            ParameterValue.Create(StartIndexParameter, request.StartIndex),
            ParameterValue.Create(TermsParameter, request.SelectedTerms is null ? null : request.SelectedTerms.Any() ? request.SelectedTerms: null)
        };
    }

    protected override Documents Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
