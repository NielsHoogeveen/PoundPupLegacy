namespace PoundPupLegacy.CreateModel.Readers;

using Request = TermReaderByNameableIdRequest;

public sealed class TermReaderByNameableIdRequest : IRequest
{
    public required int OwnerId { get; init; }
    public required string VocabularyName { get; init; }
    public required int NameableId { get; init; }
}
internal sealed class TermReaderByNameableIdFactory : SingleItemDatabaseReaderFactory<Request, Term>
{
    private static readonly NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };
    private static readonly NonNullableStringDatabaseParameter VocabularyName = new() { Name = "vocabulary_name" };
    private static readonly NonNullableIntegerDatabaseParameter NameableId = new() { Name = "nameable_id" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };
    private static readonly StringValueReader NameReader = new() { Name = "name" };
    private static readonly IntValueReader NameableIdReader = new() { Name = "nameable_id" };
    private static readonly IntValueReader VocabularyIdReader = new() { Name = "vocabulary_id" };


    public override string Sql => SQL;

    const string SQL = """
        SELECT 
            t.id, 
            t.nameable_id,
            t.name,
            t.vocabulary_id
        FROM term t
        JOIN vocabulary v on v.id = t.vocabulary_id
        WHERE v.owner_id = @owner_id AND v.name = @vocabulary_name AND nameable_id = @nameable_id
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(OwnerId, request.OwnerId),
            ParameterValue.Create(VocabularyName, request.VocabularyName),
            ParameterValue.Create(NameableId, request.NameableId)
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
}
