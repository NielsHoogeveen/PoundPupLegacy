﻿using Npgsql;
using NpgsqlTypes;
using PoundPupLegacy.Model;

namespace PoundPupLegacy.Db.Writers;

internal class CountryWriter : DatabaseWriter<Country>, IDatabaseWriter<Country>
{
    private const string ID = "id";
    private const string HAGUE_STATUS_ID = "hague_status_id";
    private const string RESIDENCY_REQUIREMENTS = "residency_requirements";
    private const string AGE_REQUIREMENTS = "age_requirements";
    private const string MARRIAGE_REQUIREMENTS = "marriage_requirements";
    private const string INCOME_REQUIREMENTS = "income_requirements";
    private const string HEALTH_REQUIREMENTS = "health_requirements";
    private const string OTHER_REQUIREMENTS = "other_requirements";
    public static DatabaseWriter<Country> Create(NpgsqlConnection connection)
    {
        var command = CreateInsertStatement(
            connection,
            "country",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = HAGUE_STATUS_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = RESIDENCY_REQUIREMENTS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = AGE_REQUIREMENTS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = MARRIAGE_REQUIREMENTS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = INCOME_REQUIREMENTS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = HEALTH_REQUIREMENTS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = OTHER_REQUIREMENTS,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
            }
        );
        return new CountryWriter(command);

    }

    internal CountryWriter(NpgsqlCommand command) : base(command)
    {
    }

    internal override void Write(Country country)
    {
        try
        {
            WriteValue(country.Id, ID);
            WriteValue(country.HagueStatusId, HAGUE_STATUS_ID);
            WriteNullableValue(country.ResidencyRequirements, RESIDENCY_REQUIREMENTS);
            WriteNullableValue(country.AgeRequirements, AGE_REQUIREMENTS);
            WriteNullableValue(country.MarriageRequirements, MARRIAGE_REQUIREMENTS);
            WriteNullableValue(country.IncomeRequirements, INCOME_REQUIREMENTS);
            WriteNullableValue(country.HealthRequirements, HEALTH_REQUIREMENTS);
            WriteNullableValue(country.OtherRequirements, OTHER_REQUIREMENTS);
            _command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
