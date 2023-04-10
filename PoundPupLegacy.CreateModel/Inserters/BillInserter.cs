namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class BillInserterFactory : BasicDatabaseInserterFactory<Bill, BillInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableDateTimeDatabaseParameter IntroductionDate = new() { Name = "introduction_date" };

    public override string TableName => "bill";

}
internal sealed class BillInserter : BasicDatabaseInserter<Bill>
{
    public BillInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Bill item)
    {
        if (item.Id is null)
            throw new ArgumentNullException(nameof(item.Id));
        return new ParameterValue[] {
            ParameterValue.Create(BillInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(BillInserterFactory.IntroductionDate, item.IntroductionDate)
        };
    }
}
