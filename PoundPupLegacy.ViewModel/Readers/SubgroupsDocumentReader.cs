using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.ViewModel.Readers;
public class SubgroupsDocumentReaderFactory : IDatabaseReaderFactory<SubgroupsDocumentReader>
{
    public async Task<SubgroupsDocumentReader> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        command.CommandType = CommandType.Text;
        command.CommandTimeout = 300;
        command.CommandText = SQL;
        command.Parameters.Add("subgroup_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("limit", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("offset", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();
        return new SubgroupsDocumentReader(command);
    }

    const string SQL = """
            select
                jsonb_build_object(
            	    'NumberOfEntries',
            	    number_of_entries,
            	    'Entries',
            	    jsonb_agg(
            		    jsonb_build_object(
            			    'Title',
            			    title,
            			    'Path',
            			    path,
            			    'Authoring', 
            			    jsonb_build_object(
            				    'Id', publisher_id, 
            				    'Name', publisher_name,
            				    'CreatedDateTime', created_date_time,
            				    'ChangedDateTime', changed_date_time
            			    ),
            			    'HasBeenPublished',
            			    case 
            				    when status = 0 then false
            				    else true
            			    end
            		    )
            	    )
                ) document
            from(
                select
                    id,
                    title,
                    node_type_id,
                    path,
                    tenant_id,
                    node_id,
                    created_date_time,
                    changed_date_time,
                    url_id,
                    publisher_id,
                    publisher_name,
                    count(id) over() number_of_entries,
                    url_path,
            	    subgroup_id,
            	    publication_status_id,
                    status
                from(
                    select
            	        tn.id,
            	        n.title,
            	        n.node_type_id,
            	        case 
            		        when tn.url_path is null then '/node/' || tn.url_id
            		        else '/' || tn.url_path
            	        end path,
            	        tn.tenant_id,
            	        tn.node_id,
            	        n.created_date_time,
            	        n.publisher_id,
            	        n.changed_date_time,
            	        tn.url_id,
            	        p.name publisher_name,
            	        count(tn.id) over() number_of_entries,
            	        case 
            		        when tn.url_path is null then '/node/' || tn.url_id
            		        else '/' || url_path
            	        end url_path,
            	        tn.subgroup_id,
            	        tn.publication_status_id,
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
            	        from tenant_node tn
            	        join subgroup s on s.id = tn.subgroup_id
            	        join node n on n.id = tn.node_id
            	        JOIN publisher p on p.id = n.publisher_id
            	        WHERE s.id = @subgroup_id
                    ) an
                    where an.status <> -1
                    
                    LIMIT @limit OFFSET @offset
                ) an
                group by number_of_entries
            """
            ;

}
public class SubgroupsDocumentReader : SingleItemDatabaseReader<SubgroupsDocumentReader.SubgroupDocumentRequest, SubgroupPagedList>
{
    public record SubgroupDocumentRequest
    {
        public required int UserId { get; init; }
        public required int SubgroupId { get; init; }
        public required int Limit { get; init; }
        public required int Offset { get; init; }

    }
    internal SubgroupsDocumentReader(NpgsqlCommand command) : base(command)
    {
    }
    public override async Task<SubgroupPagedList> ReadAsync(SubgroupDocumentRequest request)
    {
        _command.Parameters["subgroup_id"].Value = request.SubgroupId;
        _command.Parameters["user_id"].Value = request.UserId;
        _command.Parameters["limit"].Value = request.Limit;
        _command.Parameters["offset"].Value = request.Offset;
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        var subgroupPagedList = reader.GetFieldValue<SubgroupPagedList>(0);
        return subgroupPagedList;
    }


}
