﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ChildTraffickingCaseInserter : DatabaseInserter<ChildTraffickingCase>, IDatabaseInserter<ChildTraffickingCase>
{
    private const string ID = "id";
    private const string NUMBER_OF_CHILDREN_INVOLVED = "number_of_children_involved";
    private const string COUNTRY_ID_FROM = "country_id_from";
    public static async Task<DatabaseInserter<ChildTraffickingCase>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "child_trafficking_case",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = NUMBER_OF_CHILDREN_INVOLVED,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = COUNTRY_ID_FROM,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },

            }
        );
        return new ChildTraffickingCaseInserter(command);

    }

    internal ChildTraffickingCaseInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(ChildTraffickingCase abuseCase)
    {
        if (abuseCase.Id is null)
            throw new NullReferenceException();

        WriteValue(abuseCase.Id, ID);
        WriteNullableValue(abuseCase.NumberOfChildrenInvolved, NUMBER_OF_CHILDREN_INVOLVED);
        WriteValue(abuseCase.CountryIdFrom, COUNTRY_ID_FROM);
        await _command.ExecuteNonQueryAsync();
    }
}
