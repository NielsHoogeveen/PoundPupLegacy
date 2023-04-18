namespace PoundPupLegacy.CreateModel.Inserters;

using Request = PollVote;

internal sealed class PollVoteInserterFactory : BasicDatabaseInserterFactory<Request>
{
    internal static NullCheckingIntegerDatabaseParameter PollId = new() { Name = "poll_id" };
    internal static NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };
    internal static NullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
    internal static NullableStringDatabaseParameter IPAddress = new() { Name = "ip_address" };

    public override string TableName => "poll_vote";
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(PollId, request.PollId),
            ParameterValue.Create(Delta, request.Delta),
            ParameterValue.Create(UserId, request.UserId),
            ParameterValue.Create(IPAddress, request.IpAddress),
        };
    }
}
