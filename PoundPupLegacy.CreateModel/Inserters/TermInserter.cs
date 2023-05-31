namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Term.ToCreateForExistingNameable;

internal sealed class TermInserterFactory : AutoGenerateIdDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    private static readonly TrimmingNonNullableStringDatabaseParameter Name = new() { Name = "name" };
    private static readonly NonNullableIntegerDatabaseParameter NameableId = new() { Name = "nameable_id" };

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
