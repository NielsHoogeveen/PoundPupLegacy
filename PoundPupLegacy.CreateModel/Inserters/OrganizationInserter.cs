namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class OrganizationInserter : DatabaseInserter<Organization>, IDatabaseInserter<Organization>
{
    private const string ID = "id";
    private const string WEBSITE_URL = "website_url";
    private const string EMAIL_ADDRESS = "email_address";
    private const string DESCRIPTION = "description";
    private const string ESTABLISHED = "established";
    private const string TERMINATED = "terminated";
    public static async Task<DatabaseInserter<Organization>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "organization",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = WEBSITE_URL,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = EMAIL_ADDRESS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = DESCRIPTION,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = ESTABLISHED,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
                new ColumnDefinition{
                    Name = TERMINATED,
                    NpgsqlDbType = NpgsqlDbType.Timestamp
                },
            }
        );

        return new OrganizationInserter(command);

    }

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
