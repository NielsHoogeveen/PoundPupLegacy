namespace PoundPupLegacy.CreateModel.Readers;

using Request = TermReaderByNameableIdRequest;
using Factory = TermReaderByNameableIdFactory;
using Reader = TermReaderByNameableId;

public sealed class TermReaderByNameableIdRequest : IRequest
{
    public required int OwnerId { get; init; }
    public required string VocabularyName { get; init; }
    public required int NameableId { get; init; }
}
internal sealed class TermReaderByNameableIdFactory : MandatorySingleItemDatabaseReaderFactory<Request, Term, Reader>
{
    internal static NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };
    internal static NonNullableStringDatabaseParameter VocabularyName = new() { Name = "vocabulary_name" };
    internal static NonNullableIntegerDatabaseParameter NameableId = new() { Name = "nameable_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };
    internal static StringValueReader NameReader = new() { Name = "name" };
    internal static IntValueReader NameableIdReader = new() { Name = "nameable_id" };
    internal static IntValueReader VocabularyIdReader = new() { Name = "vocabulary_id" };


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
}
internal sealed class TermReaderByNameableId : MandatorySingleItemDatabaseReader<Request, Term>
{

    public TermReaderByNameableId(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.OwnerId, request.OwnerId),
            ParameterValue.Create(Factory.VocabularyName, request.VocabularyName),
            ParameterValue.Create(Factory.NameableId, request.NameableId)
        };
    }

    protected override Term Read(NpgsqlDataReader reader)
    {
        return new Term {
            Id = Factory.IdReader.GetValue(reader),
            Name = Factory.NameReader.GetValue(reader),
            VocabularyId = Factory.VocabularyIdReader.GetValue(reader),
            NameableId = Factory.NameableIdReader.GetValue(reader)
        };
    }

    protected override string GetErrorMessage(Request request)
    {
        return $"term {request.NameableId} cannot be found in vocabulary {request.VocabularyName} of owner {request.OwnerId}";
    }
}
