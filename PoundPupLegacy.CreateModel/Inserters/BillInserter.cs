namespace PoundPupLegacy.CreateModel.Inserters;

using Request  = Bill;

internal sealed class BillInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    internal static NullableDateTimeDatabaseParameter IntroductionDate = new() { Name = "introduction_date" };
    internal static NullableIntegerDatabaseParameter ActId = new() { Name = "act_id" };

    public override string TableName => "bill";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(IntroductionDate, request.IntroductionDate),
            ParameterValue.Create(ActId, request.ActId)
        };
    }
}
