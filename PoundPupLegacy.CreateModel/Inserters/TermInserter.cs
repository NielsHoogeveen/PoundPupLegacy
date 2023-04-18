namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Term;

internal sealed class TermInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    internal static TrimmingNonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter NameableId = new() { Name = "nameable_id" };

    public override string TableName => "term";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(VocabularyId, request.VocabularyId),
            ParameterValue.Create(Name, request.Name),
            ParameterValue.Create(NameableId, request.NameableId)
        };
    }
}
