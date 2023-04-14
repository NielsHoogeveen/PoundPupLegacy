namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = OrganizationInserterFactory;
using Request = Organization;
using Inserter = OrganizationInserter;

internal sealed class OrganizationInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullableStringDatabaseParameter WebsiteURL = new() { Name = "website_url" };
    internal static NullableStringDatabaseParameter EmailAddress = new() { Name = "email_address" };
    internal static NullableFuzzyDateDatabaseParameter Established = new() { Name = "established" };
    internal static NullableFuzzyDateDatabaseParameter Terminated = new() { Name = "terminated" };

    public override string TableName => "organization";

}
internal sealed class OrganizationInserter : IdentifiableDatabaseInserter<Request>
{
    public OrganizationInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.WebsiteURL, request.WebsiteUrl),
            ParameterValue.Create(Factory.EmailAddress, request.EmailAddress),
            ParameterValue.Create(Factory.Established, request.Established),
            ParameterValue.Create(Factory.Terminated, request.Terminated),
        };
    }
}
