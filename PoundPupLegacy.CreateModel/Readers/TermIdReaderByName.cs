namespace PoundPupLegacy.CreateModel.Readers;

using Request = TermIdReaderByNameRequest;

public sealed class TermIdReaderByNameRequest : IRequest
{
    public required int VocabularyId { get; init; }
    public required string Name { get; init; }
}
internal sealed class TermIdReaderByNameFactory : MandatorySingleItemDatabaseReaderFactory<Request, int>
{
    private static readonly NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    private static readonly TrimmingNonNullableStringDatabaseParameter Name = new() { Name = "name" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT 
        id
        FROM term 
        WHERE vocabulary_id = @vocabulary_id
        AND name = @name 
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(VocabularyId, request.VocabularyId),
            ParameterValue.Create(Name, request.Name)
        };
    }

    protected override int Read(NpgsqlDataReader reader)
    {
        return IdReader.GetValue(reader);
    }
    protected override string GetErrorMessage(Request request)
    {
        return $"term {request.Name} cannot be found in vocabulary {request.VocabularyId}";
    }
}
