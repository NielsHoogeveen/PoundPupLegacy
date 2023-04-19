using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Inserters;

using Request = FileInserterRequest;

public record FileInserterRequest : IRequest
{
    public required string Name { get; init; }
    public required string MimeType { get; init; }
    public required string Path { get; init; }
    public required int NodeId { get; init; }
    public required long Size { get; init; }
}

public sealed class FileInserterFactory : DatabaseInserterFactoryBase<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    private static readonly NonNullableStringDatabaseParameter MimeType = new() { Name = "mime_type" };
    private static readonly NonNullableLongDatabaseParameter Size = new() { Name = "size" };

    protected override string Sql => SQL;

    const string SQL = $"""
        insert into file (name, size, mime_type, path) VALUES(@name, @size, @mime_type, @path);
        insert into node_file (node_id, file_id) VALUES(@node_id, lastval());
        """;

    private IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Name, request.Name),
            ParameterValue.Create(Size, request.Size),
            ParameterValue.Create(MimeType, request.MimeType),
            ParameterValue.Create(Path, request.Path),
            ParameterValue.Create(NodeId, request.NodeId)
        };
    }

    protected override IDatabaseInserter<Request> CreateInstance(NpgsqlCommand command)
    {
        return new BasicDatabaseInserter<Request>(command, GetParameterValues);
    }
}

