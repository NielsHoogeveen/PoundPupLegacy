namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class CaseInserterFactory : DatabaseInserterFactory<Case, CaseInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    internal static NullableTimeStampRangeDatabaseParameter FuzzyDate = new() { Name = "fuzzy_date" };

    public override string TableName => "case";
}
internal sealed class CaseInserter : DatabaseInserter<Case>
{
    public CaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Case item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(CaseInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(CaseInserterFactory.Description, item.Description),
            ParameterValue.Create(CaseInserterFactory.FuzzyDate, item.Date),
        };
    }
}
