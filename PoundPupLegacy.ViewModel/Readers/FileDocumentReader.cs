using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.ViewModel.Readers;
public class FileDocumentReaderFactory : IDatabaseReaderFactory<FileDocumentReader>
{
    public async Task<FileDocumentReader> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;
        var command = postgresConnection.CreateCommand();

        command.CommandType = CommandType.Text;
        command.CommandText = SQL;

        command.Parameters.Add("file_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
        command.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
        await command.PrepareAsync();

        return new FileDocumentReader(command);
    }

    const string SQL = """
                    select
                        jsonb_build_object(
                            'Id', 
                            id,
                            'Name', 
                            name,
                            'MimeType', 
                            mime_type,
                            'Path', 
                            path,
                            'Size', 
                            size
                        ) document
                    from(
                        select
                            id,
                            path,
                            name,
                            mime_type,
                            size,
                            case 
                                when status = 1 then true
                                else false
                            end can_be_accessed
                        from(
                            SELECT 
                                f.id,
                                f.path,
                                f.name,
                                f.mime_type,
                                f.size,
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
                            FROM public.file f
                            join node_file nf on nf.file_id = f.id
                            join tenant_node tn on tn.node_id = nf.node_id 
                            where tn.tenant_id = @tenant_id and f.id = @file_id
                			union
                			select
                                f.id,
                                f.path,
                                f.name,
                                f.mime_type,
                                f.size,
                				1 status
                			from "file" f
                			left join node_file nf on nf.file_id = f.id
                			where f.id = @file_id
                			and nf.file_id is null
                        ) x
                        where status > -1
                    ) x
                    group by id,
                    path,
                    name,
                    mime_type,
                    size
                    having every(can_be_accessed)
                """;

}
public class FileDocumentReader : SingleItemDatabaseReader<FileDocumentReader.FileDocumentRequest, File>
{
    public record FileDocumentRequest
    {
        public int FileId { get; init; }
        public int UserId { get; init; }
        public int TenantId { get; init; }

    }
    internal FileDocumentReader(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task<File> ReadAsync(FileDocumentRequest request)
    {
        _command.Parameters["file_id"].Value = request.FileId;
        _command.Parameters["user_id"].Value = request.UserId;
        _command.Parameters["tenant_id"].Value = request.TenantId;
        await using var reader = await _command.ExecuteReaderAsync();
        await reader.ReadAsync();
        return await reader.GetFieldValueAsync<File>(0);
    }


}
