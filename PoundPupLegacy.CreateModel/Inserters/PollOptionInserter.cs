using static System.Net.Mime.MediaTypeNames;

namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PollOptionInserterFactory : DatabaseInserterFactory<PollOption>
{
    public override async Task<IDatabaseInserter<PollOption>> CreateAsync(IDbConnection connection)
    {
        if (connection is not NpgsqlConnection)
            throw new Exception("Application only works with a Postgres database");
        var postgresConnection = (NpgsqlConnection)connection;

        var command = await CreateInsertStatementAsync(
            postgresConnection,
            "poll_option",
            new ColumnDefinition[] {
                new ColumnDefinition{
                    Name = PollOptionInserter.POLL_QUESTION_ID,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PollOptionInserter.DELTA,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
                new ColumnDefinition{
                    Name = PollOptionInserter.TEXT,
                    NpgsqlDbType = NpgsqlDbType.Varchar
                },
                new ColumnDefinition{
                    Name = PollOptionInserter.NUMBER_OF_VOTES,
                    NpgsqlDbType = NpgsqlDbType.Integer
                },
            }
        );
        return new PollOptionInserter(command);
    }
}
internal sealed class PollOptionInserter : DatabaseInserter<PollOption>
{
    internal const string POLL_QUESTION_ID = "poll_question_id";
    internal const string DELTA = "delta";
    internal const string TEXT = "text";
    internal const string NUMBER_OF_VOTES = "number_of_votes";

    internal PollOptionInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override async Task InsertAsync(PollOption pollOption)
    {
        if (pollOption.PollQuestionId is null)
            throw new NullReferenceException();
        SetParameter(pollOption.PollQuestionId, POLL_QUESTION_ID);
        SetParameter(pollOption.Delta, DELTA);
        SetNullableParameter(pollOption.Text, TEXT);
        SetNullableParameter(pollOption.NumberOfVotes, NUMBER_OF_VOTES);
        await _command.ExecuteNonQueryAsync();
    }
}
