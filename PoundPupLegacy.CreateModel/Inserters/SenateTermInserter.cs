﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class SenateTermInserter : DatabaseInserter<SenateTerm>, IDatabaseInserter<SenateTerm>
{
    private const string ID = "id";
    private const string SENATOR_ID = "senator_id";
    private const string SUBDIVISION_ID = "subdivision_id";
    private const string DATE_RANGE = "date_range";
    public static async Task<DatabaseInserter<SenateTerm>> CreateAsync(NpgsqlConnection connection)
    {
        var command = await CreateInsertStatementAsync(
            connection,
            "senate_term",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SENATOR_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = SUBDIVISION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new SenateTermInserter(command);

    }

    internal SenateTermInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(SenateTerm senateTerm)
    {
        if (senateTerm.Id is null)
            throw new NullReferenceException();
        if (senateTerm.SenatorId is null)
            throw new NullReferenceException();
        WriteValue(senateTerm.Id, ID);
        WriteValue(senateTerm.SenatorId, SENATOR_ID);
        WriteValue(senateTerm.SubdivisionId, SUBDIVISION_ID);
        WriteDateTimeRange(senateTerm.DateTimeRange, DATE_RANGE);
        await _command.ExecuteNonQueryAsync();
    }
}