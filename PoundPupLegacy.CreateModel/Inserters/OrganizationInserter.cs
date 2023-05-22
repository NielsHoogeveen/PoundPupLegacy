namespace PoundPupLegacy.CreateModel.Inserters;

using Request = EventuallyIdentifiableOrganization;

internal sealed class OrganizationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullableStringDatabaseParameter WebsiteURL = new() { Name = "website_url" };
    private static readonly NullableStringDatabaseParameter EmailAddress = new() { Name = "email_address" };
    private static readonly NullableFuzzyDateDatabaseParameter Established = new() { Name = "established" };
    private static readonly NullableFuzzyDateDatabaseParameter Terminated = new() { Name = "terminated" };

    public override string TableName => "organization";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(WebsiteURL, request.WebsiteUrl),
            ParameterValue.Create(EmailAddress, request.EmailAddress),
            ParameterValue.Create(Established, request.Established),
            ParameterValue.Create(Terminated, request.Terminated),
        };
    }
}
