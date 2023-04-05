namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class OrganizationInserterFactory : DatabaseInserterFactory<Organization>
{
    public override async Task<IDatabaseInserter<Organization>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "organization",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = OrganizationInserter.ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = OrganizationInserter.WEBSITE_URL,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = OrganizationInserter.EMAIL_ADDRESS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = OrganizationInserter.DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = OrganizationInserter.ESTABLISHED,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
                new ColumnDefinition{
                    Name = OrganizationInserter.TERMINATED,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
            }
        );
        return new OrganizationInserter(command);
    }
}
internal sealed class OrganizationInserter : DatabaseInserter<Organization>
{
    internal const string ID = "id";
    internal const string WEBSITE_URL = "website_url";
    internal const string EMAIL_ADDRESS = "email_address";
    internal const string DESCRIPTION = "description";
    internal const string ESTABLISHED = "established";
    internal const string TERMINATED = "terminated";

    internal OrganizationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(Organization organization)
    {
        if (organization.Id is null)
            throw new NullReferenceException();

        WriteValue(organization.Id, ID);
        WriteNullableValue(organization.WebsiteURL, WEBSITE_URL);
        WriteNullableValue(organization.EmailAddress, EMAIL_ADDRESS);
        WriteNullableValue(organization.Description, DESCRIPTION);
        WriteNullableValue(organization.Established, ESTABLISHED);
        WriteNullableValue(organization.Terminated, TERMINATED);
        await _command.ExecuteNonQueryAsync();
    }
}
