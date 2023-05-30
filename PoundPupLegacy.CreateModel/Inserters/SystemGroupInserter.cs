namespace PoundPupLegacy.CreateModel.Inserters;

using Request = SystemGroup;

internal sealed class SystemGroupInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullCheckingIntegerDatabaseParameter VocabularyIdTagging = new() { Name = "vocabulary_id_tagging" };

    public override string TableName => "system_group";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(VocabularyIdTagging, request.IdentificationForCreate?.Id),
        };
    }
}
