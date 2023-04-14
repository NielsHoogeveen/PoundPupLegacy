namespace PoundPupLegacy.CreateModel.Inserters;

using Factory  = BillInserterFactory;
using Request  = Bill;
using Inserter = BillInserter;

internal sealed class BillInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NullableDateTimeDatabaseParameter IntroductionDate = new() { Name = "introduction_date" };

    public override string TableName => "bill";

}
internal sealed class BillInserter : IdentifiableDatabaseInserter<Request>
{
    public BillInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.IntroductionDate, request.IntroductionDate)
        };
    }
}
