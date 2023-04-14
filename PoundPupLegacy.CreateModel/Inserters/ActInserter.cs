namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = ActInserterFactory;
using Request = Act;
using Inserter = ActInserter;

internal sealed class ActInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullableDateTimeDatabaseParameter EnactmentDate = new() { Name = "enactment_date" };

    public override string TableName => "act";
}
internal sealed class ActInserter : IdentifiableDatabaseInserter<Request>
{
    public ActInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.EnactmentDate, request.EnactmentDate)
        };
    }
}
