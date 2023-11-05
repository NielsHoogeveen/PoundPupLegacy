namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = CoercedAdoptionCasesDocumentReaderRequest;

public sealed record CoercedAdoptionCasesDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int StartIndex { get; init; }
    public required int Length { get; init; }

    public required int[] SelectedTerms { get; init; }
}
internal sealed class CoercedAdoptionCasesDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, CoercedAdoptionCases>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter LengthParameter = new() { Name = "length" };
    private static readonly NonNullableIntegerDatabaseParameter StartIndexParameter = new() { Name = "start_index" };
    private static readonly NullableIntegerArrayDatabaseParameter TermsParameter = new() { Name = "terms" };

    private static readonly FieldValueReader<CoercedAdoptionCases> DocumentReader = new() { Name = "document" };

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
                            'Date',
                            fuzzy_date,
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
                        has_been_published,
                        publication_status_id,
                        fuzzy_date,
                        terms
                        from(
        			        select
        				        node_id,
                                count(*) over() number_of_elements,
        				        title,
        				        teaser,
        				        path,
                                changed_date_time,
        				        case 
        					        when publication_status_id = 0 then false
        					        else true
        				        end has_been_published,
                                publication_status_id,
                                fuzzy_date,
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
        				        t.id term_id,
        				        nmt.node_id,
        				        n.title,
                                n.changed_date_time,
                                tn.publication_status_id,
        				        nm.description teaser,
                                cs.fuzzy_date,
        				        '/' || nt.viewer_path ||'/'  || tn.node_id path,
                                t.name term_name,
                                '/' || nt2.viewer_path ||'/'  || tn2.node_id term_path,
                                nmt2.tag_label_name term_type_name,
        				        case 
        					        when n.node_type_id = 41 then 1
        					        when n.node_type_id = 23 then 1
        					        else 1
        				        end weight
        				        from node n
                                join node_type nt on n.node_type_id = nt.id
                                join nameable nm on nm.id = n.id
                                join "case" cs on cs.id = n.id
                                join coerced_adoption_case ac on ac.id = n.id
        				        join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id
        				        join node_term nmt on nmt.node_id = n.id 
        				        join term t on t.id = nmt.term_id
                                join node n2 on n2.id = t.nameable_id
                                join node_type nt2 on n2.node_type_id = nt2.id
                                left join nameable_type nmt2 on nmt2.id = n2.node_type_id and nmt2.tag_label_name is not null
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
                                        nmt.node_id,
                                        count(*) over() c
                                        from term t
                                        left join node_term nmt on nmt.term_id = t.id and nmt.node_id = n.id
                                        where t.id = ANY(@terms)
                                    ) x
                                    group by node_id, c
                                    having count(node_id) = c
                                ))
        			        ) x
        			        GROUP BY 
        				        node_id,
        				        title,
        				        path,
        				        teaser,
        				        has_been_published,
                                publication_status_id,
                                changed_date_time,
                                fuzzy_date
                        ) x
        			    ORDER BY lower(fuzzy_date) desc
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
        				    t.name title,
        				    case 
        					    when n.node_type_id = 41 then 1
        					    when n.node_type_id = 23 then 1
        					    else 1
        				    end weight
        			    from node n
        			    join term t on t.nameable_id = n.id 
        			    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id
        			    join vocabulary v on v.id = t.vocabulary_id
        			    join node_term nmt on nmt.term_id = t.id
        			    join node n2 on n2.id = nmt.node_id
                        join coerced_adoption_case ac on ac.id = n2.id
        			    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
        			    where v.name = 'Topics'
                        and tn.publication_status_id in 
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
        			    and (@terms is null or n2.id in (
                            select
                            node_id
                            from (
                                select
                                nmt.node_id,
                                count(*) over() c
                                from term t
                                left join node_term nmt on nmt.term_id = t.id and nmt.node_id = n2.id
                                where t.id = ANY(@terms)
                            ) x
                            group by node_id, c
                            having count(node_id) = c
                        ))
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
            ParameterValue.Create(TermsParameter, request.SelectedTerms is null ? null : request.SelectedTerms.Any() ? request.SelectedTerms.ToArray(): null)
        };
    }

    protected override CoercedAdoptionCases Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
