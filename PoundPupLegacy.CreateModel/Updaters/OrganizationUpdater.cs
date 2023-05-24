namespace PoundPupLegacy.CreateModel.Updaters;

using Request = ImmediatelyIdentifiableOrganization;

internal sealed class OrganizationUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };
    private static readonly NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    private static readonly NullableStringDatabaseParameter WebsiteUrl = new() { Name = "website_url" };
    private static readonly NullableStringDatabaseParameter EmailAddress = new() { Name = "email_address" };
    private static readonly NullableFuzzyDateDatabaseParameter Established = new() { Name = "established" };
    private static readonly NullableFuzzyDateDatabaseParameter Terminated = new() { Name = "terminated" };

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
            ParameterValue.Create(NodeId, request.Id),
            ParameterValue.Create(Title, request.Title),
            ParameterValue.Create(Description, request.Description),
            ParameterValue.Create(WebsiteUrl, request.WebsiteUrl),
            ParameterValue.Create(EmailAddress, request.EmailAddress),
            ParameterValue.Create(Established, request.Established),
            ParameterValue.Create(Terminated, request.Terminated)
        };
    }
}

