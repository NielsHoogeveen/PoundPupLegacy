namespace PoundPupLegacy.CreateModel.Inserters;
internal sealed class PollStatusInserterFactory : BasicDatabaseInserterFactory<PollStatus, PollStatusInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Name = new() { Name = "name" };

    public override string TableName => "poll_status";

}
internal sealed class PollStatusInserter : BasicDatabaseInserter<PollStatus>
{
    public PollStatusInserter(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(PollStatus item)
    {
        if (item.Id is null)
            throw new NullReferenceException();
        return new ParameterValue[] {
            ParameterValue.Create(PollStatusInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(PollStatusInserterFactory.Name, item.Name),
        };
    }
}
