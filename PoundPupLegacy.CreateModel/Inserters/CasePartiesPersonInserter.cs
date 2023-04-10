namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CasePartiesPersonInserterFactory : DatabaseInserterFactory<CasePartiesPerson, CasePartiesPersonInserter>
{
    internal static NonNullableIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    internal static NonNullableIntegerDatabaseParameter PersonId = new() { Name = "person_id" };

    public override string TableName => "case_parties_person";

}
internal sealed class CasePartiesPersonInserter : DatabaseInserter<CasePartiesPerson>
{
    public CasePartiesPersonInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(CasePartiesPerson item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CasePartiesPersonInserterFactory.CasePartiesId, item.CasePartiesId),
            ParameterValue.Create(CasePartiesPersonInserterFactory.PersonId, item.PersonId)
        };
    }
}
