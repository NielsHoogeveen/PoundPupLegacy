namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = CreateNodeActionInserterFactory;
using Request = CreateNodeAction;
using Inserter = CreateNodeActionInserter;

internal sealed class CreateNodeActionInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override string TableName => "create_node_action";

}
internal sealed class CreateNodeActionInserter : IdentifiableDatabaseInserter<Request>
{
    public CreateNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeTypeId, request.NodeTypeId),
        };
    }
}
