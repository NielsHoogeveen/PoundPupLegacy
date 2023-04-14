namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class PollInserterFactory : DatabaseInserterFactory<Poll, PollInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableDateTimeDatabaseParameter DateTimeClosure = new() { Name = "date_time_closure" };
    internal static NonNullableIntegerDatabaseParameter PollStatusId = new() { Name = "poll_status_id" };

    public override string TableName => "poll";


}
internal sealed class PollInserter : DatabaseInserter<Poll>
{
    public PollInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Poll item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
                   ParameterValue.Create(PollInserterFactory.Id, item.Id.Value),
                   ParameterValue.Create(PollInserterFactory.DateTimeClosure, item.DateTimeClosure),
                   ParameterValue.Create(PollInserterFactory.PollStatusId, item.PollStatusId),
               };
    }

}
