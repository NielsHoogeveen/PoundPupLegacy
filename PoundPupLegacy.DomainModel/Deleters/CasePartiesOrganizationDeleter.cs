namespace PoundPupLegacy.DomainModel.Deleters;

using Request = CasePartiesOrganizationDeleterRequest;

public sealed record CasePartiesOrganizationDeleterRequest : IRequest
{
    public required int CasePartiesId { get; init; }
    public required int OrganizationId { get; init; }
}

internal sealed class CasePartiesOrganizationDeleterFactory : DatabaseDeleterFactory<Request>
{
    private static NonNullableIntegerDatabaseParameter CasePartiesId = new() { Name = "case_parties_id" };
    private static NonNullableIntegerDatabaseParameter OrganizationId = new() { Name = "organization_id" };


    public override string Sql => SQL;

    const string SQL = $"""
        delete from case_parties_organization
        where organization_id = @organization_id and case_parties_id = @case_parties_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(CasePartiesId, request.CasePartiesId),
            ParameterValue.Create(OrganizationId, request.OrganizationId),
       };
    }
}
