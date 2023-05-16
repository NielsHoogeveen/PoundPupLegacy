namespace PoundPupLegacy.EditModel.Updaters;

using Request = OrganizationUpdaterRequest;

public sealed record OrganizationUpdaterRequest : IRequest
{
    public required int NodeId { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required string? WebsiteUrl { get; init; }
    public required string? EmailAddress { get; init; }
    public required DateTimeRange? EstablishmentDateRange { get; init; }
    public required DateTimeRange? TerminationDateRange { get; init; }

}
internal sealed class OrganizationUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    private static readonly NullableStringDatabaseParameter WebsiteUrl = new() { Name = "website_url" };
    private static readonly NullableStringDatabaseParameter EmailAddress = new() { Name = "email_address" };
    private static readonly NullableDateRangeDatabaseParameter Established = new() { Name = "established" };
    private static readonly NullableDateRangeDatabaseParameter Terminated = new() { Name = "terminated" };

    public override string Sql => $"""
        update node 
        set 
            title=@title
        where id = @node_id;
        update nameable 
        set 
            description=@description
        where id = @node_id;
        update organization 
        set 
            website_url=@website_url,
            email_address=@email_address,
            established=@established,
            terminated=@terminated
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(Title, request.Title),
            ParameterValue.Create(Description, request.Description),
            ParameterValue.Create(WebsiteUrl, request.WebsiteUrl),
            ParameterValue.Create(EmailAddress, request.EmailAddress),
            ParameterValue.Create(Established, request.EstablishmentDateRange),
            ParameterValue.Create(Terminated, request.TerminationDateRange)
        };
    }
}

