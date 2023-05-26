namespace PoundPupLegacy.CreateModel.Readers;

using Request = TermIdReaderByNameableIdRequest;

public sealed class TermIdReaderByNameableIdRequest : IRequest
{
    public required int VocabularyId { get; init; }
    public required int NameableId { get; init; }
}
internal sealed class TermIdReaderByNameableIdFactory : MandatorySingleItemDatabaseReaderFactory<Request, int>
{
    private static readonly NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    private static readonly NonNullableIntegerDatabaseParameter NameableId = new() { Name = "nameable_id" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT 
            id, 
        FROM term t
        WHERE nameable_id = @nameable_id and vocabulary_id = @vocabulary_id
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(VocabularyId, request.VocabularyId),
            ParameterValue.Create(NameableId, request.NameableId)
        };
    }

    protected override int Read(NpgsqlDataReader reader)
    {
        return IdReader.GetValue(reader);
    }
    protected override string GetErrorMessage(Request request)
    {
        return $"term for nameable {request.NameableId} cannot be found in vocabulary {request.VocabularyId}";
    }
}
