namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PollVoteInserterFactory : DatabaseInserterFactory<PollVote, PollVoteInserter> 
{
    internal static NonNullableIntegerDatabaseParameter PollId = new() { Name = "poll_id" };
    internal static NonNullableIntegerDatabaseParameter Delta = new() { Name = "delta" };
    internal static NullableIntegerDatabaseParameter UserId = new() { Name = "user_id" };
    internal static NullableStringDatabaseParameter IPAddress = new() { Name = "ip_address" };

    public override string TableName => "poll_vote";
}
internal sealed class PollVoteInserter : DatabaseInserter<PollVote>
{

    public PollVoteInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(PollVote item)
    {
        if (item.PollId is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(PollVoteInserterFactory.PollId, item.PollId.Value),
            ParameterValue.Create(PollVoteInserterFactory.Delta, item.Delta),
            ParameterValue.Create(PollVoteInserterFactory.UserId, item.UserId),
            ParameterValue.Create(PollVoteInserterFactory.IPAddress, item.IpAddress),
        };
    }
}
