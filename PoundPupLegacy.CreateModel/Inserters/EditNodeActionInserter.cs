namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = EditNodeActionInserterFactory;
using Request = EditNodeAction;
using Inserter = EditNodeActionInserter;

internal sealed class EditNodeActionInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override string TableName => "edit_node_action";
}
internal sealed class EditNodeActionInserter : IdentifiableDatabaseInserter<Request>
{
    public EditNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request item)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeTypeId, item.NodeTypeId),
        };
    }
}
