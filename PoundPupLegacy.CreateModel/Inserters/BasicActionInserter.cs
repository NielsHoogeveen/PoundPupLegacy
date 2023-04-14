namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = BasicActionInserterFactory;
using Request = BasicAction;
using Inserter = BasicActionInserter;

internal sealed class BasicActionInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableStringDatabaseParameter Path = new() { Name = "path" };
    internal static NonNullableStringDatabaseParameter Description = new() { Name = "description" };
    public override string TableName => "basic_action";
}
internal sealed class BasicActionInserter : IdentifiableDatabaseInserter<Request>
{
    public BasicActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Path, request.Path),
            ParameterValue.Create(Factory.Description, request.Description)
        };
    }
}
