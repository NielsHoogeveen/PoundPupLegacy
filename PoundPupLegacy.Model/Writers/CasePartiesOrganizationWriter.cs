namespace PoundPupLegacy.CreateModel.Writers;

internal sealed class CasePartiesOrganizationWriter : DatabaseWriter<CasePartiesOrganization>, IDatabaseWriter<CasePartiesOrganization>
{

    private const string CASE_PARTIES_ID = "case_parties_id";
    private const string ORGANIZATION_ID = "organization_id";
    public static async Task<DatabaseWriter<CasePartiesOrganization>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "case_parties_organization",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = CASE_PARTIES_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = ORGANIZATION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new CasePartiesOrganizationWriter(command);

    }

    internal CasePartiesOrganizationWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override async Task WriteAsync(CasePartiesOrganization casePartiesOrganization)
    {
        WriteValue(casePartiesOrganization.CasePartiesId, CASE_PARTIES_ID);
        WriteValue(casePartiesOrganization.OrganizationId, ORGANIZATION_ID);
        await _command.ExecuteNonQueryAsync();
    }
}
