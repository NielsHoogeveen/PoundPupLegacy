namespace PoundPupLegacy.CreateModel.Inserters;

internal sealed class BasicActionInserterFactory : DatabaseInserterFactory<BasicAction, BasicActionInserter>
{
    internal static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };
    internal static NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    public override string TableName => "basic_action";
}
internal sealed class BasicActionInserter : DatabaseInserter<BasicAction>
{
    public BasicActionInserter(NpgsqlCommand command) : base(command)
    {
    }
    public override IEnumerable<ParameterValue> GetParameterValues(BasicAction item)
    {
        if (!item.Id.HasValue) {
            throw new NullReferenceException();
        }
        return new ParameterValue[] {
            ParameterValue.Create(BasicActionInserterFactory.Id, item.Id.Value),
            ParameterValue.Create(BasicActionInserterFactory.Path, item.Path),
            ParameterValue.Create(BasicActionInserterFactory.Description, item.Description)
        };
    }
}
