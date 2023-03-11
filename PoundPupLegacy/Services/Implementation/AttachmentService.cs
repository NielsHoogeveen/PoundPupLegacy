using Microsoft.AspNetCore.Components.Forms;
using Npgsql;
using OneOf;
using OneOf.Types;
using PoundPupLegacy.Db;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Services.Implementation;

public class AttachmentService : IAttachmentService
{
    private readonly NpgsqlConnection _connection;
    private readonly ILogger<AttachmentService> _logger;
    private readonly IConfiguration _configuration;

    public AttachmentService(
        NpgsqlConnection connection,
        IConfiguration configuration,
        ILogger<AttachmentService> logger)
    {
        _connection = connection;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<int?> StoreFile(IFormFile file)
    {
        var attachmentsLocation = _configuration["AttachmentsLocation"];
        if (attachmentsLocation is null) {
            _logger.LogError("AttachmentsLocation is not defined in appsettings.json");
            return null;
        }
        var maxFileSizeString = _configuration["MaxFileSize"];
        if (maxFileSizeString is null) {
            _logger.LogError("Max file size is not defined in appsettings.json");
            return null;
        }
        if (int.TryParse(maxFileSizeString, out int maxFileSize)) {
            await _connection.OpenAsync();
            try {
                var fileName = Guid.NewGuid().ToString();
                var fullName = attachmentsLocation + "\\" + fileName;
                await using FileStream fs = new(fullName, FileMode.Create);
                await file.CopyToAsync(fs);
                var fm = new Model.File {
                    Id = null,
                    MimeType = file.ContentType,
                    Name = file.FileName,
                    Path = fileName,
                    Size = (int)file.Length,
                    TenantFiles = new List<TenantFile>()
                };
                await FileCreator.CreateAsync(new List<Model.File> { fm }.ToAsyncEnumerable(), _connection);
                return fm.Id;
            }finally
            { 
                await _connection.CloseAsync();
            }
        }
        else {
            _logger.LogError("Max file size as defined in appsettings.json is not a number");
            return null;
        }
    }
    public async Task<string?> StoreFile(IBrowserFile file)
    {
        var attachmentsLocation = _configuration["AttachmentsLocation"];
        if (attachmentsLocation is null) {
            _logger.LogError("AttachmentsLocation is not defined in appsettings.json");
            return null;
        }
        var maxFileSizeString = _configuration["MaxFileSize"];
        if(maxFileSizeString is null) {
            _logger.LogError("Max file size is not defined in appsettings.json");
            return null;
        }
        if(int.TryParse(maxFileSizeString, out int maxFileSize)) {
            var fileName = Guid.NewGuid().ToString();
            var fullName = attachmentsLocation + "\\" + fileName;
            await using FileStream fs = new(fullName, FileMode.Create);
            await file.OpenReadStream(maxFileSize).CopyToAsync(fs);
            return fileName;
        }
        else {
            _logger.LogError("Max file size as defined in appsettings.json is not a number");
            return null;
        }
    }

    public async Task<OneOf<FileReturn, None, Error<string>>> GetFileStream(int id, int userId, int tenantId)
    {
        await _connection.OpenAsync();
        try {
            using var command = _connection.CreateCommand();
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = """
                    select
                        name,
                        mime_type,
                        path
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

            command.Parameters.Add("file_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("user_id", NpgsqlTypes.NpgsqlDbType.Integer);
            command.Parameters.Add("tenant_id", NpgsqlTypes.NpgsqlDbType.Integer);
            await command.PrepareAsync();
            command.Parameters["file_id"].Value = id;
            command.Parameters["user_id"].Value = userId;
            command.Parameters["tenant_id"].Value = tenantId;
            var reader = await command.ExecuteReaderAsync();
            if (!reader.HasRows) {
                _logger.LogError($"No file found for id {id}");
                return new None();
            }
            await reader.ReadAsync();
            var attachementsLocation = _configuration["AttachmentsLocation"];
            if(attachementsLocation is null) {
                _logger.LogError("AttachmentsLocation is not defined in appsettings.json");
                return new None();
            }
            var fullPath = attachementsLocation + "\\" + reader.GetString(2);
            var ret = new FileReturn { FileName = reader.GetString(0), MimeType = reader.GetString(1), Stream = System.IO.File.OpenRead(fullPath) };
            await reader.CloseAsync();
            return ret;
        }
        catch(Exception e) {
            return new Error<string>(e.Message);
        }
        finally { 
            await _connection.CloseAsync(); 
        }
    }
}
