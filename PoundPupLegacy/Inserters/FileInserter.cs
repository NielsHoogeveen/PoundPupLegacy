using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Inserters;

public record FileInserterRequest
{
    public required string Name { get; init; }
    public required string MimeType { get; init; }
    public required string Path { get; init; }
    public required int NodeId { get; init; }
    public required long Size { get; init; }
}

public sealed class FileInserterFactory : DatabaseInserterFactoryBase<FileInserterRequest, FileInserter>
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

public sealed class FileInserter : DatabaseInserter<FileInserterRequest>
{
    public FileInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(FileInserterRequest request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(FileInserterFactory.Name, request.Name),
            ParameterValue.Create(FileInserterFactory.Size, request.Size),
            ParameterValue.Create(FileInserterFactory.MimeType, request.MimeType),
            ParameterValue.Create(FileInserterFactory.Path, request.Path),
            ParameterValue.Create(FileInserterFactory.NodeId, request.NodeId)
        };
    }
}
