namespace PoundPupLegacy.CreateModel.Inserters;

using Request = EventuallyIdentifiableVocabulary;

internal sealed class VocabularyInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };
    private static readonly NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };

    public override string TableName => "vocabulary";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(OwnerId, request.OwnerId),
            ParameterValue.Create(Name, request.Name),
            ParameterValue.Create(Description, request.Description),
        };
    }
}
