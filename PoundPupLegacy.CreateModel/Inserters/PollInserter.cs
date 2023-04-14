namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PollInserterFactory;
using Request = Poll;
using Inserter = PollInserter;

internal sealed class PollInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableDateTimeDatabaseParameter DateTimeClosure = new() { Name = "date_time_closure" };
    internal static NonNullableIntegerDatabaseParameter PollStatusId = new() { Name = "poll_status_id" };

    public override string TableName => "poll";


}
internal sealed class PollInserter : IdentifiableDatabaseInserter<Request>
{
    public PollInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.DateTimeClosure, request.DateTimeClosure),
            ParameterValue.Create(Factory.PollStatusId, request.PollStatusId),
        };
    }
}
