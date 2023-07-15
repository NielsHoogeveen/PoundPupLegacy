namespace PoundPupLegacy.DomainModel.Deleters;

using Request = FileDeleterRequest;

public sealed record FileDeleterRequest : IRequest
{
    public required int NodeId { get; init; }
    public required int FileId { get; init; }
}

internal sealed class FileDeleterFactory : DatabaseDeleterFactory<Request>
{

    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableIntegerDatabaseParameter FileId = new() { Name = "file_id" };

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
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(FileId, request.FileId),
        };
    }
}
