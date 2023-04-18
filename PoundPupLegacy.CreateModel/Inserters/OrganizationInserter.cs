namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Organization;

internal sealed class OrganizationInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NullableStringDatabaseParameter WebsiteURL = new() { Name = "website_url" };
    internal static NullableStringDatabaseParameter EmailAddress = new() { Name = "email_address" };
    internal static NullableFuzzyDateDatabaseParameter Established = new() { Name = "established" };
    internal static NullableFuzzyDateDatabaseParameter Terminated = new() { Name = "terminated" };

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
