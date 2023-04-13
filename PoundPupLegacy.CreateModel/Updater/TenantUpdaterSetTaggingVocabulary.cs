namespace PoundPupLegacy.CreateModel.Updaters;

public sealed class TenantUpdaterSetTaggingVocabularyFactory : DatabaseUpdaterFactory<TenantUpdaterSetTaggingVocabulary>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() {
        Name = "tenant_id"
    };
    internal static NonNullableIntegerDatabaseParameter VocabularyId = new() {
        Name = "vocabulary_id"
    };
    public override string Sql => """
        UPDATE tenant 
        SET 
        vocabulary_id_tagging = @vocabulary_id 
        WHERE id = @tenant_id
        """;

}
public sealed class TenantUpdaterSetTaggingVocabulary : DatabaseUpdater<TenantUpdaterSetTaggingVocabulary.Request>
{
    public record Request
    {
        public required int TenantId { get; init; }
        public required int VocabularyId { get; init; }
    }

    public TenantUpdaterSetTaggingVocabulary(NpgsqlCommand command) : base(command) { }

    public override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(TenantUpdaterSetTaggingVocabularyFactory.TenantId, request.TenantId),
            ParameterValue.Create(TenantUpdaterSetTaggingVocabularyFactory.VocabularyId, request.VocabularyId),
        };
    }
}
