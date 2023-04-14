namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = TermInserterFactory;
using Request = Term;
using Inserter = TermInserter;

internal sealed class TermInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    internal static TrimmingNonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter NameableId = new() { Name = "nameable_id" };

    public override string TableName => "term";


}
internal sealed class TermInserter : AutoGenerateIdDatabaseInserter<Request>
{

    public TermInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.VocabularyId, request.VocabularyId),
            ParameterValue.Create(Factory.Name, request.Name),
            ParameterValue.Create(Factory.NameableId, request.NameableId)
        };
    }
}
