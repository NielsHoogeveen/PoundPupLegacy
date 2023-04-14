namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = DeleteNodeActionInserterFactory;
using Request = DeleteNodeAction;
using Inserter = DeleteNodeActionInserter;

internal sealed class DeleteNodeActionInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override string TableName => "delete_node_action";
}
internal sealed class DeleteNodeActionInserter : IdentifiableDatabaseInserter<Request>
{

    public DeleteNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeTypeId, item.NodeTypeId),
        };
    }
}
