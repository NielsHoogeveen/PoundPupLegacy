namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PollOption;

internal sealed class PollOptionInserterFactory : BasicDatabaseInserterFactory<Request>
{
    internal static NullCheckingIntegerDatabaseParameter PollQuesyionId = new() { Name = "poll_question_id" };
    internal static NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableIntegerDatabaseParameter NumberOfVotes = new() { Name = "number_of_votes" };

    public override string TableName => "poll_option";

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PollQuesyionId, request.PollQuestionId),
            ParameterValue.Create(Delta, request.Delta),
            ParameterValue.Create(Text, request.Text),
            ParameterValue.Create(NumberOfVotes, request.NumberOfVotes),
        };
    }
}
