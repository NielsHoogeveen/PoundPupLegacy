namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = CasePartiesPersonInserterFactory;
using Request = CasePartiesPerson;
using Inserter = CasePartiesPersonInserter;

internal sealed class CasePartiesPersonInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    internal static NonNullableIntegerDatabaseParameter PersonId = new() { Name = "person_id" };

    public override string TableName => "case_parties_person";

}
internal sealed class CasePartiesPersonInserter : DatabaseInserter<Request>
{
    public CasePartiesPersonInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.CasePartiesId, request.CasePartiesId),
            ParameterValue.Create(Factory.PersonId, request.PersonId)
        };
    }
}
