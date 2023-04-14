namespace PoundPupLegacy.CreateModel.Updaters;

using Request = TenantUpdaterSetTaggingVocabularyRequest;
using Factory = TenantUpdaterSetTaggingVocabularyFactory;
using Updater = TenantUpdaterSetTaggingVocabulary;

public record TenantUpdaterSetTaggingVocabularyRequest: IRequest
{
    public required int TenantId { get; init; }
    public required int VocabularyId { get; init; }
}

public sealed class TenantUpdaterSetTaggingVocabularyFactory : DatabaseUpdaterFactory<Request, Updater>
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
public sealed class TenantUpdaterSetTaggingVocabulary : DatabaseUpdater<Request>
{
    public TenantUpdaterSetTaggingVocabulary(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(Factory.TenantId, request.TenantId),
            ParameterValue.Create(Factory.VocabularyId, request.VocabularyId),
        };
    }
}
