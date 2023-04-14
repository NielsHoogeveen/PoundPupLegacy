namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class TermInserterFactory : AutoGenerateIdDatabaseInserterFactory<Term, TermInserter>
{
    internal static NonNullableIntegerDatabaseParameter VocabularyId = new() { Name = "vocabulary_id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };
    internal static NonNullableIntegerDatabaseParameter NameableId = new() { Name = "nameable_id" };

    public override string TableName => "term";


}
internal sealed class TermInserter : AutoGenerateIdDatabaseInserter<Term>
{

    public TermInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Term term)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TermInserterFactory.VocabularyId, term.VocabularyId),
            ParameterValue.Create(TermInserterFactory.Name, term.Name.Trim()),
            ParameterValue.Create(TermInserterFactory.NameableId, term.NameableId)
        };
    }
}
