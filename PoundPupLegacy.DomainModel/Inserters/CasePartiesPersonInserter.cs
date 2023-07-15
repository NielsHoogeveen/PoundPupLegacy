using PoundPupLegacy.DomainModel;

namespace PoundPupLegacy.DomainModel.Inserters;

using Request = CasePartiesPerson;

internal sealed class CasePartiesPersonInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    private static readonly NonNullableIntegerDatabaseParameter PersonId = new() { Name = "person_id" };

    public override string TableName => "case_parties_person";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CasePartiesId, request.CasePartiesId),
            ParameterValue.Create(PersonId, request.PersonId)
        };
    }
}
