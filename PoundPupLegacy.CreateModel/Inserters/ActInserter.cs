namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class ActInserterFactory : DatabaseInserterFactory<Act, ActInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NullableDateTimeDatabaseParameter EnactmentDate = new() { Name = "enactment_date" };

    public override string TableName => "act";
}
internal sealed class ActInserter : DatabaseInserter<Act>
{
    public ActInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Act act)
    {
        if (act.Id is null)
            throw new ArgumentNullException(nameof(act.Id));

        return new ParameterValue[] {
            ParameterValue.Create(ActInserterFactory.Id, act.Id.Value),
            ParameterValue.Create(ActInserterFactory.EnactmentDate, act.EnactmentDate)
        };
    }
}
