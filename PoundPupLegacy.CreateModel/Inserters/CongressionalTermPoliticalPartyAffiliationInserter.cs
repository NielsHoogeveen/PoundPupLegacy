﻿namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class CongressionalTermPoliticalPartyAffiliationInserter : DatabaseInserter<CongressionalTermPoliticalPartyAffiliation>, IDatabaseInserter<CongressionalTermPoliticalPartyAffiliation>
{
    private const string ID = "id";
    private const string CONGRESSIONAL_TERM_ID = "congressional_term_id";
    private const string UNITED_STATES_POLITICAL_PARTY_AFFLIATION_ID = "united_states_political_party_affiliation_id";
    private const string DATE_RANGE = "date_range";
    public static async Task<DatabaseInserter<CongressionalTermPoliticalPartyAffiliation>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "congressional_term_political_party_affiliation",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = CONGRESSIONAL_TERM_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = UNITED_STATES_POLITICAL_PARTY_AFFLIATION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = DATE_RANGE,
                    NpgsqlDbType = NpgsqlDbType.Unknown
                },
            }
        );
        return new CongressionalTermPoliticalPartyAffiliationInserter(command);

    }

    internal CongressionalTermPoliticalPartyAffiliationInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(CongressionalTermPoliticalPartyAffiliation congressionalTermPoliticalPartyAffiliation)
    {
        if (congressionalTermPoliticalPartyAffiliation.Id is null)
            throw new NullReferenceException();
        WriteValue(congressionalTermPoliticalPartyAffiliation.Id, ID);
        WriteValue(congressionalTermPoliticalPartyAffiliation.CongressionalTermId, CONGRESSIONAL_TERM_ID);
        WriteValue(congressionalTermPoliticalPartyAffiliation.PoliticalPartyAffiliationId, UNITED_STATES_POLITICAL_PARTY_AFFLIATION_ID);
        WriteDateTimeRange(congressionalTermPoliticalPartyAffiliation.DateTimeRange, DATE_RANGE);
        await _command.ExecuteNonQueryAsync();
    }
}
