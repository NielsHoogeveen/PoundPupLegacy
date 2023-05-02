namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = FathersRightsViolationCasesDocumentReaderRequest;

public sealed record FathersRightsViolationCasesDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int Limit { get; init; }
    public required int Offset { get; init; }
}
internal sealed class FathersRightsViolationCasesDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, FathersRightsViolationCases>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter LimitParameter = new() { Name = "limit" };
    private static readonly NonNullableIntegerDatabaseParameter OffsetParameter = new() { Name = "offset" };

    private static readonly FieldValueReader<FathersRightsViolationCases> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    private const string SQL = """
            select
            	jsonb_build_object(
            		'NumberOfEntries', 
                    number_of_entries,
            		'Entries', 
                    jsonb_agg(
            			jsonb_build_object
            			(
            				'Title', 
                            title,
                            'Path', 
                            url_path,
            				'Text', 
                            description,
                            'DateTimeRange',
                            jsonb_build_object(
                                'From',
                                date_from,
                                'To',
                                date_to
                            ),
                        	'HasBeenPublished', 
                            case 
            					when status = 0 then false
            					else true
            				end,
                            'Tags',
                            (
                                select jsonb_agg
                                (
                                    jsonb_build_object(
                                        'Path',
                                        case when tn.url_path is null then '/node/' || tn.url_id
                                            else '/' || url_path
                                        end,
                                        'Title',
                                        n.title,
                                        'NodeTypeName',
                                        nty.tag_label_name
                                    )
                                )
                                from node_term nt
                                join term t on t.id = nt.term_id
                                join node n on n.id = t.nameable_id
                                left join nameable_type nty on nty.id = n.node_type_id
                                join tenant_node tn on tn.node_id = n.id and tn.tenant_id = @tenant_id
                                where nt.node_id = an.id
            
                            ) 
            			)
            		)
            	) "document"
            from(
            	select
            	*
            	from(
            		select
                    n.id,
            		n.title,
            		c.description,
            		nt.name node_type_name,
            		n.node_type_id,
            		COUNT(*) OVER() number_of_entries,
            		case 
            			when tn.url_path is null then '/node/' || tn.url_id
            			else '/' || url_path
            		end url_path,
            		lower(c.fuzzy_date) date_from,
                    upper(c.fuzzy_date) date_to,
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
            			end status	
            		from
            		tenant_node tn
            		join node n on n.id = tn.node_id
            		join "case" c on c.id = n.id
                    join fathers_rights_violation_case ac on ac.id = c.id
            		join node_type nt on nt.id = n.node_type_id
            		WHERE tn.tenant_id = @tenant_id
            	) an
            	order by date_from desc
            	LIMIT @limit OFFSET @offset
            ) an
            where an.status <> -1
            group by number_of_entries
            """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
            ParameterValue.Create(LimitParameter, request.Limit),
            ParameterValue.Create(OffsetParameter, request.Offset),
        };
    }

    protected override FathersRightsViolationCases Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
