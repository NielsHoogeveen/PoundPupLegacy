namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = FileInserterFactory;
using Request = File;
using Inserter = FileInserter;

internal sealed class FileInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter MimeType = new() { Name = "mime_type" };
    internal static NonNullableIntegerDatabaseParameter Size = new() { Name = "size" };

    public override string TableName => "file";
}

internal sealed class FileInserter : ConditionalAutoGenerateIdDatabaseInserter<Request>
{
    public FileInserter(NpgsqlCommand command, NpgsqlCommand autoGenerateIdCommand) : base(command, autoGenerateIdCommand)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Path, request.Path),
            ParameterValue.Create(Factory.Name, request.Name),
            ParameterValue.Create(Factory.MimeType, request.MimeType),
            ParameterValue.Create(Factory.Size, request.Size)
        };
    }
}
