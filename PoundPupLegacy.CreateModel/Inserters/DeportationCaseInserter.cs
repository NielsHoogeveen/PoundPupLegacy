﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class DeportationCaseInserter : DatabaseInserter<DeportationCase>, IDatabaseInserter<DeportationCase>
{
    private const string ID = "id";
    private const string SUBDIVISION_ID_FROM = "subdivision_id_from";
    private const string COUNTRY_ID_TO = "country_id_to";
    public static async Task<DatabaseInserter<DeportationCase>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "deportation_case",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SUBDIVISION_ID_FROM,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = COUNTRY_ID_TO,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new DeportationCaseInserter(command);

    }

    internal DeportationCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(DeportationCase deportationCase)
    {
        if (deportationCase.Id is null)
            throw new NullReferenceException();

        WriteValue(deportationCase.Id, ID);
        WriteNullableValue(deportationCase.SubdivisionIdFrom, SUBDIVISION_ID_FROM);
        WriteNullableValue(deportationCase.CountryIdTo, COUNTRY_ID_TO);
        await _command.ExecuteNonQueryAsync();
    }
}
