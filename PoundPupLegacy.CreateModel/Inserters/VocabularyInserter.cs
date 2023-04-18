namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Vocabulary;

internal sealed class VocabularyInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };

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
