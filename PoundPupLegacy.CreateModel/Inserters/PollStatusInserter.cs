namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = PollStatusInserterFactory;
using Request = PollStatus;
using Inserter = PollStatusInserter;

internal sealed class PollStatusInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "poll_status";

}
internal sealed class PollStatusInserter : IdentifiableDatabaseInserter<Request>
{
    public PollStatusInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Name, request.Name),
        };
    }
}
