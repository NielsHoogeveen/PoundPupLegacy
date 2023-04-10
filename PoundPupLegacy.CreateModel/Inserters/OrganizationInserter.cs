namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class OrganizationInserterFactory : DatabaseInserterFactory<Organization, OrganizationInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableStringDatabaseParameter WebsiteURL = new() { Name = "website_url" };
    internal static NullableStringDatabaseParameter EmailAddress = new() { Name = "email_address" };
    internal static NullableFuzzyDateDatabaseParameter Established = new() { Name = "established" };
    internal static NullableFuzzyDateDatabaseParameter Terminated = new() { Name = "terminated" };

    public override string TableName => "organization";

}
internal sealed class OrganizationInserter : DatabaseInserter<Organization>
{
    public OrganizationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Organization item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(OrganizationInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(OrganizationInserterFactory.WebsiteURL, item.WebsiteUrl),
            ParameterValue.Create(OrganizationInserterFactory.EmailAddress, item.EmailAddress),
            ParameterValue.Create(OrganizationInserterFactory.Established, item.Established),
            ParameterValue.Create(OrganizationInserterFactory.Terminated, item.Terminated),
        };
    }
}
