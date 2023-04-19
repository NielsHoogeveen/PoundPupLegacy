namespace PoundPupLegacy.CreateModel.Inserters;

using Request = Act;

internal sealed class ActInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NullableDateTimeDatabaseParameter EnactmentDate = new() { Name = "enactment_date" };

    public override string TableName => "act";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(EnactmentDate, request.EnactmentDate)
        };
    }
}
