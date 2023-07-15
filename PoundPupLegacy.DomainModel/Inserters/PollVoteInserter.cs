namespace PoundPupLegacy.DomainModel.Inserters;

using Request = PollVote;

internal sealed class PollVoteInserterFactory : BasicDatabaseInserterFactory<Request>
{
    private static readonly NullCheckingIntegerDatabaseParameter PollId = new() { Name = "poll_id" };
    private static readonly NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };
    private static readonly NullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
    private static readonly NullableStringDatabaseParameter IPAddress = new() { Name = "ip_address" };

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
