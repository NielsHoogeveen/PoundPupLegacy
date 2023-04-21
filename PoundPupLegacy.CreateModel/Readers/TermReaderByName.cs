namespace PoundPupLegacy.CreateModel.Readers;

using Request = TermReaderByNameRequest;

public sealed class TermReaderByNameRequest : IRequest
{
    public required int VocabularyId { get; init; }
    public required string Name { get; init; }
}
internal sealed class TermReaderByNameFactory : MandatorySingleItemDatabaseReaderFactory<Request, Term>
{
    private static readonly NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    private static readonly TrimmingNonNullableStringDatabaseParameter Name = new() { Name = "name" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };
    private static readonly StringValueReader NameReader = new() { Name = "name" };
    private static readonly IntValueReader NameableIdReader = new() { Name = "nameable_id" };
    private static readonly IntValueReader VocabularyIdReader = new() { Name = "vocabulary_id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT 
        id, 
        name,
        nameable_id,
        vocabulary_id
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

    protected override Term Read(NpgsqlDataReader reader)
    {
        return new Term {
            Id = IdReader.GetValue(reader),
            Name = NameReader.GetValue(reader),
            VocabularyId = VocabularyIdReader.GetValue(reader),
            NameableId = NameableIdReader.GetValue(reader)
        };
    }
    protected override string GetErrorMessage(Request request)
    {
        return $"term {request.Name} cannot be found in vocabulary {request.VocabularyId}";
    }
}
