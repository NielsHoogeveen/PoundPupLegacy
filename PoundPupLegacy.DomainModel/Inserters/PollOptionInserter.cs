namespace PoundPupLegacy.DomainModel.Inserters;

using Request = PollOption;

internal sealed class PollOptionInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NullCheckingIntegerDatabaseParameter PollQuesyionId = new() { Name = "poll_question_id" };
    private static readonly NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };
    private static readonly NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    private static readonly NonNullableIntegerDatabaseParameter NumberOfVotes = new() { Name = "number_of_votes" };

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
