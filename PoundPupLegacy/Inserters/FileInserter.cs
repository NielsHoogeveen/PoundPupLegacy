using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Inserters;

using Request = FileInserterRequest;
using Factory = FileInserterFactory;
using Inserter = FileInserter;

public record FileInserterRequest: IRequest
{
    public required string Name { get; init; }
    public required string MimeType { get; init; }
    public required string Path { get; init; }
    public required int NodeId { get; init; }
    public required long Size { get; init; }
}

public sealed class FileInserterFactory : DatabaseInserterFactoryBase<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter MimeType = new() { Name = "mime_type" };
    internal static NonNullableLongDatabaseParameter Size = new() { Name = "size" };

    protected override string Sql => SQL;

    const string SQL = $"""
        insert into file (name, size, mime_type, path) VALUES(@name, @size, @mime_type, @path);
        insert into node_file (node_id, file_id) VALUES(@node_id, lastval());
        """;
}

public sealed class FileInserter : DatabaseInserter<Request>
{
    public FileInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Name, request.Name),
            ParameterValue.Create(Factory.Size, request.Size),
            ParameterValue.Create(Factory.MimeType, request.MimeType),
            ParameterValue.Create(Factory.Path, request.Path),
            ParameterValue.Create(Factory.NodeId, request.NodeId)
        };
    }
}
