namespace PoundPupLegacy.CreateModel.Readers;

using Factory = TermReaderByNameFactory;
using Reader = TermReaderByName;
public sealed class TermReaderByNameFactory : DatabaseReaderFactory<Reader>
{
    internal static NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    internal static IntValueReader IdReader = new() { Name = "id" };
    internal static StringValueReader NameReader = new() { Name = "name" };
    internal static IntValueReader NameableIdReader = new() { Name = "nameable_id" };
    internal static IntValueReader VocabularyIdReader = new() { Name = "vocabulary_id" };

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
}
public sealed class TermReaderByName : MandatorySingleItemDatabaseReader<Reader.Request, Term>
{
    public record Request
    {
        public required int VocabularyId { get; init; }
        public required string Name { get; init; }

    }

    internal TermReaderByName(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.VocabularyId, request.VocabularyId),
            ParameterValue.Create(Factory.Name, request.Name)
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
        return $"term {request.Name} cannot be found in vocabulary {request.VocabularyId}";
    }
}
