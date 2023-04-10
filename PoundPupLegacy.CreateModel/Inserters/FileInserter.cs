using System.Collections.Immutable;

namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class FileInserterFactory: ConditionalAutoGenerateIdDatabaseInserterFactory<File, FileInserter>
{
    internal static AutoGenerateIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter MimeType = new() { Name = "mime_type" };
    internal static NonNullableIntegerDatabaseParameter Size = new() { Name = "size" };

    public override string TableName => "file";
}

internal sealed class FileInserter : ConditionalAutoGenerateIdDatabaseInserter<File>
{
    public FileInserter(NpgsqlCommand command, NpgsqlCommand autoGenerateIdCommand) : base(command, autoGenerateIdCommand)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(File item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(FileInserterFactory.Id, item.Id),
            ParameterValue.Create(FileInserterFactory.Path, item.Path),
            ParameterValue.Create(FileInserterFactory.Name, item.Name),
            ParameterValue.Create(FileInserterFactory.MimeType, item.MimeType),
            ParameterValue.Create(FileInserterFactory.Size, item.Size)
        };
    }
}
