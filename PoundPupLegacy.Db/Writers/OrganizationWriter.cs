﻿using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class OrganizationWriter : DatabaseWriter<Organization>, IDatabaseWriter<Organization>
{
    private const string ID = "id";
    private const string WEBSITE_URL = "website_url";
    private const string EMAIL_ADDRESS = "email_address";
    private const string DESCRIPTION = "description";
    private const string ESTABLISHED = "established";
    private const string TERMINATED = "terminated";
    public static DatabaseWriter<Organization> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
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

        return new OrganizationWriter(command);

    }

    internal OrganizationWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Organization organization)
    {
        WriteValue(organization.Id, ID);
        WriteNullableValue(organization.WebsiteURL, WEBSITE_URL);
        WriteNullableValue(organization.EmailAddress, EMAIL_ADDRESS);
        WriteNullableValue(organization.Description, DESCRIPTION);
        WriteNullableValue(organization.Established, ESTABLISHED);
        WriteNullableValue(organization.Terminated, TERMINATED);
        _command.ExecuteNonQuery();
    }
}
