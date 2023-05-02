namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.Common;
using PoundPupLegacy.ViewModel.Models;
using Request = CasesDocumentReaderRequest;

public sealed record CasesDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int Limit { get; init; }
    public required int Offset { get; init; }
}
internal sealed class CasesDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, Cases>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter LimitParameter = new() { Name = "limit" };
    private static readonly NonNullableIntegerDatabaseParameter OffsetParameter = new() { Name = "offset" };

    private static readonly FieldValueReader<Cases> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    private const string SQL = """
            select
            	jsonb_build_object(
                    'CaseTypes',
                    (
                        select jsonb_agg
                        (
                            jsonb_build_object(
                            'Path', 
            				ba.path,
                            'Title', 
                            nt.name,
                            'Text',
                            ct.text
                            )
                        ) 
                        from case_type ct
                        join node_type nt on nt.id = ct.id
            			join view_node_type_list_action nta on nta.node_type_id = nt.id
            			join basic_action ba on ba.id = nta.basic_action_id
                    ),
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
            				'DateFrom', 
                            date_from,
            				'DateTo', 
                            date_to,
            				'CaseType',	
                            node_type_name,
            				'HasBeenPublished', 
                            case 
            					when status = 0 then false
            					else true
            				end 
            			)
            		)
            	) "document"
            from(
            	select
            	*
            	from(
            		select
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

    protected override Cases Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
