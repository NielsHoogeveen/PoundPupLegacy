namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = VocabularyInserterFactory;
using Request = Vocabulary;
using Inserter = VocabularyInserter;

internal sealed class VocabularyInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };

    public override string TableName => "vocabulary";

}
internal sealed class VocabularyInserter : IdentifiableDatabaseInserter<Request>
{
    public VocabularyInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.OwnerId, request.OwnerId),
            ParameterValue.Create(Factory.Name, request.Name),
            ParameterValue.Create(Factory.Description, request.Description),
        };
    }
}
