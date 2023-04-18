namespace PoundPupLegacy.CreateModel.Inserters;

using Request = File;

internal sealed class FileInserterFactory : ConditionalAutoGenerateIdDatabaseInserterFactory<Request>
{
    internal static NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter MimeType = new() { Name = "mime_type" };
    internal static NonNullableIntegerDatabaseParameter Size = new() { Name = "size" };

    public override string TableName => "file";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Path, request.Path),
            ParameterValue.Create(Name, request.Name),
            ParameterValue.Create(MimeType, request.MimeType),
            ParameterValue.Create(Size, request.Size)
        };
    }

}

