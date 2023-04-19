namespace PoundPupLegacy.CreateModel.Updaters;

using Factory = TenantUpdaterSetTaggingVocabularyFactory;
using Request = TenantUpdaterSetTaggingVocabularyRequest;

public record TenantUpdaterSetTaggingVocabularyRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int VocabularyId { get; init; }
}

public sealed class TenantUpdaterSetTaggingVocabularyFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    public override string Sql => """
        UPDATE tenant 
        SET 
        vocabulary_id_tagging = @vocabulary_id 
        WHERE id = @tenant_id
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(Factory.TenantId, request.TenantId),
            ParameterValue.Create(Factory.VocabularyId, request.VocabularyId),
        };
    }

}
