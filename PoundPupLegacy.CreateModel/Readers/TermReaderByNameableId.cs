namespace PoundPupLegacy.DomainModel.Readers;

using Request = TermNameReaderByNameableIdRequest;

public sealed class TermNameReaderByNameableIdRequest : IRequest
{
    public required int VocabularyId { get; init; }
    public required int NameableId { get; init; }
}
public sealed class TermNameReaderByNameableIdFactory : MandatorySingleItemDatabaseReaderFactory<Request, string>
{
    private static readonly NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    private static readonly NonNullableIntegerDatabaseParameter NameableId = new() { Name = "nameable_id" };

    private static readonly StringValueReader NameReader = new() { Name = "name" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT 
        name
        FROM term
        WHERE vocabulary_id = @vocabulary_id AND nameable_id = @nameable_id
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(VocabularyId, request.VocabularyId),
            ParameterValue.Create(NameableId, request.NameableId)
        };
    }

    protected override string Read(NpgsqlDataReader reader)
    {
        return NameReader.GetValue(reader);
    }

    protected override string GetErrorMessage(Request request)
    {
        return $"Couldn't find term name for nameable {request.NameableId} in vocabulary {request.VocabularyId}";
    }
}
