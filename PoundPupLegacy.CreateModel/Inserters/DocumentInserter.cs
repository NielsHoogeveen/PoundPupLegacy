namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = DocumentInserterFactory;
using Request = Document;
using Inserter = DocumentInserter;

internal sealed class DocumentInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullableFuzzyDateDatabaseParameter Published = new() { Name = "published" };
    internal static NullableStringDatabaseParameter SourceUrl = new() { Name = "source_url" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };
    internal static NullableIntegerDatabaseParameter DocumentTypeId = new() { Name = "document_type_id" };

    public override string TableName => "document";
}
internal sealed class DocumentInserter : IdentifiableDatabaseInserter<Request>
{
    public DocumentInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Text, request.Text),
            ParameterValue.Create(Factory.Teaser, request.Teaser),
            ParameterValue.Create(Factory.Published, request.PublicationDate),
            ParameterValue.Create(Factory.SourceUrl, request.SourceUrl),
            ParameterValue.Create(Factory.DocumentTypeId, request.DocumentTypeId),
        };
    }
}
