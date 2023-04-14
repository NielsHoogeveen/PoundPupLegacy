using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Deleters;

using Request = FileDeleterRequest;
using Factory = FileDeleterFactory;
using Deleter = FileDeleter;

public record FileDeleterRequest: IRequest
{
    public required int NodeId { get; init; }
    public required int FileId { get; init; }
}

internal sealed class FileDeleterFactory : DatabaseDeleterFactory<Request,Deleter>
{

    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableIntegerDatabaseParameter FileId = new() { Name = "file_id" };

    public override string Sql => SQL;

    const string SQL = """
        delete from node_file
        where file_id = @file_id and node_id = @node_id;
        delete from tenant_file
        where file_id in (
            select 
            id 
            from file f
            left join node_file nf on nf.file_id = f.id
            where nf.file_id is null
            and f.id = @file_id
        );
        delete from file
        where id in (
            select 
            id 
            from file f
            left join node_file nf on nf.file_id = f.id
            where nf.file_id is null
            and f.id = @file_id
        );
        """;
    

}
internal sealed class FileDeleter : DatabaseDeleter<Request>
{
    public FileDeleter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeId, request.NodeId),
            ParameterValue.Create(Factory.FileId, request.FileId),
        };
    }
}
