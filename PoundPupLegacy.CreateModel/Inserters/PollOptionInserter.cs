namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PollOptionInserterFactory;
using Request = PollOption;
using Inserter = PollOptionInserter;

internal sealed class PollOptionInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NullCheckingIntegerDatabaseParameter PollQuesyionId = new() { Name = "poll_question_id" };
    internal static NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableIntegerDatabaseParameter NumberOfVotes = new() { Name = "number_of_votes" };

    public override string TableName => "poll_option";
}
internal sealed class PollOptionInserter : DatabaseInserter<Request>
{
    public PollOptionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.PollQuesyionId, request.PollQuestionId),
            ParameterValue.Create(Factory.Delta, request.Delta),
            ParameterValue.Create(Factory.Text, request.Text),
            ParameterValue.Create(Factory.NumberOfVotes, request.NumberOfVotes),
        };
    }
}
