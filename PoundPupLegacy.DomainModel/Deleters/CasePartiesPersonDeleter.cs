namespace PoundPupLegacy.DomainModel.Deleters;

using Request = CasePartiesPersonDeleterRequest;

public sealed record CasePartiesPersonDeleterRequest : IRequest
{
    public required int CasePartiesId { get; init; }
    public required int PersonId { get; init; }
}

internal sealed class CasePartiesPersonDeleterFactory : DatabaseDeleterFactory<Request>
{
    private static NonNullableIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    private static NonNullableIntegerDatabaseParameter PersonId = new() { Name = "person_id" };


    public override string Sql => SQL;

    const string SQL = $"""
        delete from case_parties_person
        where person_id = @person_id and case_parties_id = @case_parties_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CasePartiesId, request.CasePartiesId),
            ParameterValue.Create(PersonId, request.PersonId),
       };
    }
}
