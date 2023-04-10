namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class VocabularyInserterFactory : BasicDatabaseInserterFactory<Vocabulary, VocabularyInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableIntegerDatabaseParameter OwnerId = new() { Name = "owner_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };

    public override string TableName => "vocabulary";

}
internal sealed class VocabularyInserter : BasicDatabaseInserter<Vocabulary>
{
    public VocabularyInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Vocabulary item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(VocabularyInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(VocabularyInserterFactory.OwnerId, item.OwnerId),
            ParameterValue.Create(VocabularyInserterFactory.Name, item.Name),
            ParameterValue.Create(VocabularyInserterFactory.Description, item.Description),
        };
    }
}
