namespace PoundPupLegacy.DomainModel.Readers;

using Request = PossibleTermReaderByNameableIdRequest;

public sealed class PossibleTermReaderByNameableIdRequest : IRequest
{
    public required int VocabularyId { get; init; }
    public required int NameableId { get; init; }
}
public sealed class PossibleTerm : IRequest
{
    public required int TermId { get; init; }
}
internal sealed class PossibleTermReaderByNameableIdFactory : SingleItemDatabaseReaderFactory<Request, PossibleTerm>
{
    private static readonly NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    private static readonly NonNullableIntegerDatabaseParameter NameableId = new() { Name = "nameable_id" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT 
            id
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

    protected override PossibleTerm Read(NpgsqlDataReader reader)
    {
        return new PossibleTerm { TermId = IdReader.GetValue(reader) };
    }
}
