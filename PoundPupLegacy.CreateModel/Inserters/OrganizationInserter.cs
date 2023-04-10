namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class OrganizationInserterFactory : DatabaseInserterFactory<Organization>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableStringDatabaseParameter WebsiteURL = new() { Name = "website_url" };
    internal static NullableStringDatabaseParameter EmailAddress = new() { Name = "email_address" };
    internal static NullableTimeStampRangeDatabaseParameter Established = new() { Name = "established" };
    internal static NullableTimeStampRangeDatabaseParameter Terminated = new() { Name = "terminated" };


    public override async Task<IDatabaseInserter<Organization>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "organization",
            new DatabaseParameter[] {
                Id,
                WebsiteURL,
                EmailAddress,
                Established,
                Terminated
            }
        );
        return new OrganizationInserter(command);
    }
}
internal sealed class OrganizationInserter : DatabaseInserter<Organization>
{
    internal OrganizationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Organization organization)
    {
        if (organization.Id is null)
            throw new NullReferenceException();

        Set(OrganizationInserterFactory.Id, organization.Id.Value);
        Set(OrganizationInserterFactory.WebsiteURL, organization.WebsiteUrl);
        Set(OrganizationInserterFactory.EmailAddress, organization.EmailAddress);
        Set(OrganizationInserterFactory.Established, organization.Established);
        Set(OrganizationInserterFactory.Terminated, organization.Terminated);
        await _command.ExecuteNonQueryAsync();
    }
}
