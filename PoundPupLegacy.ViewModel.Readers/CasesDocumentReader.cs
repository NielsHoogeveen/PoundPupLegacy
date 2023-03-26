using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.ViewModel.Readers;

public class CasesDocumentReader : DatabaseReader, ISingleItemDatabaseReader<CasesDocumentReader, CasesDocumentReader.CasesDocumentRequest, Cases>
{
    public record CasesDocumentRequest
    {
        public int TenantId { get; init; }
        public int UserId { get; init; }
        public int Limit { get; init; }
        public int Offset { get; init; }
        public CaseType CaseType { get; init; }
    }
    private CasesDocumentReader(NpgsqlCommand command) : base(command)
    {
    }
    public async Task<Cases> ReadAsync(CasesDocumentRequest request)
    {
        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["user_id"].Value = request.UserId;
        _command.Parameters["limit"].Value = request.Limit;
        _command.Parameters["offset"].Value = request.Offset;
        if (request.CaseType == CaseType.Any) {
            _command.Parameters["node_type_id"].Value = DBNull.Value;
        }
        else {
            _command.Parameters["node_type_id"].Value = (int)request.CaseType;
        }
        await using var reader = await _command.ExecuteReaderAsync();
        if (reader.HasRows) {
            await reader.ReadAsync();
            var organizations = reader.GetFieldValue<Cases>(0);
            return organizations!;
        }
        else {

            return new Cases {
                CaseListEntries = new CaseListEntry[] { },
                NumberOfEntries = 0,
            };
        }

    }
    public static async Task<CasesDocumentReader> CreateAsync(NpgsqlConnection connection)
    {
        var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        command.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("limit", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("offset", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("node_type_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new CasesDocumentReader(command);
    }


    private const string SQL = """
            select
            	jsonb_build_object(
            		'NumberOfEntries', 
                    number_of_entries,
            		'CaseListEntries', 
                    jsonb_agg(
            			jsonb_build_object
            			(
            				'Title', 
                            title,
                            'Path', 
                            url_path,
            				'Text', 
                            description,
            				'Date', 
                            date,
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
            		case 
            			when c.date is not null then c.date
            			else lower(c.date_range)
            		end date,
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
                    AND (@node_type_id is null or n.node_type_id = @node_type_id)
            	) an
            	order by date desc
            	LIMIT @limit OFFSET @offset
            ) an
            where an.status <> -1
            group by number_of_entries
            """;

}
