﻿namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = DeportationCasesDocumentReaderRequest;

public sealed record DeportationCasesDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int StartIndex { get; init; }
    public required int Length { get; init; }

    public required int[] SelectedTerms { get; init; }
}
internal sealed class DeportationCasesDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, DeportationCases>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter LengthParameter = new() { Name = "length" };
    private static readonly NonNullableIntegerDatabaseParameter StartIndexParameter = new() { Name = "start_index" };
    private static readonly NullableIntegerArrayDatabaseParameter TermsParameter = new() { Name = "terms" };

    private static readonly FieldValueReader<DeportationCases> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
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
                            'DateTimeRange',
                            jsonb_build_object(
                                'From',
                                lower(fuzzy_date),
                                'To',
                                upper(fuzzy_date)
                            ),
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
        					        when status = 1 then true
        					        else false
        				        end has_been_published,
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
        				        nt.node_id,
        				        n.title,
                                n.changed_date_time,
        				        cs.description teaser,
                                cs.fuzzy_date,
        				        case
        					        when tn.url_path is null then '/node/' || tn.url_id
        					        else '/' || tn.url_path
        				        end path,
        			            case
        					        when tn.publication_status_id = 0 then (
        					        select
        						        case 
        							        when count(*) > 0 then 0
        							        else -1
        						        end status
        					        from user_group_user_role_user ugu
        					        join user_group ug on ug.id = ugu.user_group_id
        					        WHERE ugu.user_group_id = 
        					        case
        						        when tn.subgroup_id is null then tn.tenant_id 
        						        else tn.subgroup_id 
        					        end 
        					        AND ugu.user_role_id = ug.administrator_role_id
        					        AND ugu.user_id = @user_id
        				        )
        				        when tn.publication_status_id = 1 then 1
        				        when tn.publication_status_id = 2 then (
        					        select
        						        case 
        							        when count(*) > 0 then 1
        							        else -1
        						        end status
        					        from user_group_user_role_user ugu
        					        WHERE ugu.user_group_id = 
        						        case
        							        when tn.subgroup_id is null then tn.tenant_id 
        							        else tn.subgroup_id 
        						        end
        						        AND ugu.user_id = @user_id
        				        )
        				        end status,
                                t.name term_name,
                                case 
                                    when tn2.url_path is null then '/node/' || tn2.url_id
                                    else '/' || tn2.url_path
                                end term_path,
                                nt2.tag_label_name term_type_name,
        				        case 
        					        when n.node_type_id = 41 then 1
        					        when n.node_type_id = 23 then 1
        					        else 1
        				        end weight
        				        from node n
                                join "case" cs on cs.id = n.id
                                join deportation_case ac on ac.id = n.id
        				        join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id
        				        join node_term nt on nt.node_id = n.id 
        				        join term t on t.id = nt.term_id
                                join node n2 on n2.id = t.nameable_id
                                left join nameable_type nt2 on nt2.id = n2.node_type_id
                                join tenant_node tn2 on tn2.node_id = t.nameable_id and tn2.tenant_id = @tenant_id
        				        where (@terms is null or n.id in (
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
        			        ) x
        			        WHERE status > 0
        			        GROUP BY 
        				        node_id,
        				        title,
        				        path,
        				        teaser,
        				        has_been_published,
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
        					    when tn.publication_status_id = 0 then (
        					    select
        						    case 
        							    when count(*) > 0 then 0
        							    else -1
        						    end status
        					    from user_group_user_role_user ugu
        					    join user_group ug on ug.id = ugu.user_group_id
        					    WHERE ugu.user_group_id = 
        					    case
        						    when tn.subgroup_id is null then tn.tenant_id 
        						    else tn.subgroup_id 
        					    end 
        					    AND ugu.user_role_id = ug.administrator_role_id
        					    AND ugu.user_id = @user_id
        				    )
        				    when tn.publication_status_id = 1 then 1
        				    when tn.publication_status_id = 2 then (
        					    select
        						    case 
        							    when count(*) > 0 then 1
        							    else -1
        						    end status
        					    from user_group_user_role_user ugu
        					    WHERE ugu.user_group_id = 
        						    case
        							    when tn.subgroup_id is null then tn.tenant_id 
        							    else tn.subgroup_id 
        						    end
        						    AND ugu.user_id = @user_id
        				    )
        				    end status_term,
        				    case
        					    when tn2.publication_status_id = 0 then (
        					    select
        						    case 
        							    when count(*) > 0 then 0
        							    else -1
        						    end status
        					    from user_group_user_role_user ugu
        					    join user_group ug on ug.id = ugu.user_group_id
        					    WHERE ugu.user_group_id = 
        					    case
        						    when tn2.subgroup_id is null then tn2.tenant_id 
        						    else tn2.subgroup_id 
        					    end 
        					    AND ugu.user_role_id = ug.administrator_role_id
        					    AND ugu.user_id = @user_id
        				    )
        				    when tn2.publication_status_id = 1 then 1
        				    when tn2.publication_status_id = 2 then (
        					    select
        						    case 
        							    when count(*) > 0 then 1
        							    else -1
        						    end status
        					    from user_group_user_role_user ugu
        					    WHERE ugu.user_group_id = 
        						    case
        							    when tn2.subgroup_id is null then tn2.tenant_id 
        							    else tn2.subgroup_id 
        						    end
        						    AND ugu.user_id = @user_id
        				    )
        				    end status_node,		
        				    case 
        					    when n.node_type_id = 41 then 1
        					    when n.node_type_id = 23 then 1
        					    else 1
        				    end weight
        			    from node n
        			    join term t on t.nameable_id = n.id 
        			    join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id
        			    join vocabulary v on v.id = t.vocabulary_id
        			    join node_term nt on nt.term_id = t.id
        			    join node n2 on n2.id = nt.node_id
                        join deportation_case ac on ac.id = n2.id
        			    join tenant_node tn2 on tn2.node_id = n2.id and tn2.tenant_id = @tenant_id
        			    where v.name = 'Topics'
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
        		    ) x
        		    where status_node > 0 and status_term > 0
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

    protected override DeportationCases Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
