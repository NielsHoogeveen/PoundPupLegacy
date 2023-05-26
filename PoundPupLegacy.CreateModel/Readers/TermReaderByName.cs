namespace PoundPupLegacy.CreateModel.Readers;

using Request = TermReaderByNameRequest;

public sealed class TermReaderByNameRequest : IRequest
{
    public required int VocabularyId { get; init; }
    public required string Name { get; init; }
}
public sealed class TermReaderByNameFactory : MandatorySingleItemDatabaseReaderFactory<Request, ImmediatelyIdentifiableTerm>
{
    private static readonly NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    private static readonly TrimmingNonNullableStringDatabaseParameter Name = new() { Name = "name" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };
    private static readonly StringValueReader NameReader = new() { Name = "name" };
    private static readonly IntValueReader NameableIdReader = new() { Name = "nameable_id" };
    private static readonly IntValueReader VocabularyIdReader = new() { Name = "vocabulary_id" };
    private static readonly IntListValueReader ParentTermIdsReader = new() { Name = "parent_term_ids" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT 
            t.id, 
            t.name,
            t.nameable_id,
            t.vocabulary_id,
            array_remove(array_agg(th.term_id_parent), null) as parent_term_ids
        FROM term t
        left join term_hierarchy th on t.id = th.term_id_child
        WHERE t.vocabulary_id = @vocabulary_id
        AND name = @name 
        GROUP BY     
            t.id, 
            t.nameable_id,
            t.name,
            t.vocabulary_id
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(VocabularyId, request.VocabularyId),
            ParameterValue.Create(Name, request.Name)
        };
    }

    protected override ImmediatelyIdentifiableTerm Read(NpgsqlDataReader reader)
    {
        return new ExistingTerm {
            Id = IdReader.GetValue(reader),
            Name = NameReader.GetValue(reader),
            VocabularyId = VocabularyIdReader.GetValue(reader),
            NameableId = NameableIdReader.GetValue(reader),
            ParentTermIds = ParentTermIdsReader.GetValue(reader),
        };
    }
    protected override string GetErrorMessage(Request request)
    {
        return $"term {request.Name} cannot be found in vocabulary {request.VocabularyId}";
    }
}
