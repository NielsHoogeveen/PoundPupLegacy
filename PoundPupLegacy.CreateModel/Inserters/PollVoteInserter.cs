namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PollVoteInserterFactory;
using Request = PollVote;
using Inserter = PollVoteInserter;

internal sealed class PollVoteInserterFactory : DatabaseInserterFactory<Request, Inserter>
{
    internal static NullCheckingIntegerDatabaseParameter PollId = new() { Name = "poll_id" };
    internal static NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };
    internal static NullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
    internal static NullableStringDatabaseParameter IPAddress = new() { Name = "ip_address" };

    public override string TableName => "poll_vote";
}
internal sealed class PollVoteInserter : DatabaseInserter<Request>
{

    public PollVoteInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.PollId, request.PollId),
            ParameterValue.Create(Factory.Delta, request.Delta),
            ParameterValue.Create(Factory.UserId, request.UserId),
            ParameterValue.Create(Factory.IPAddress, request.IpAddress),
        };
    }
}
