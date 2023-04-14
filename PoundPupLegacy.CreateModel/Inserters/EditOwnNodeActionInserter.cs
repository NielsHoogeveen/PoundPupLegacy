namespace PoundPupLegacy.CreateModel.Inserters;

using Factory = EditOwnNodeActionInserterFactory;
using Request = EditOwnNodeAction;
using Inserter = EditOwnNodeActionInserter;

internal sealed class EditOwnNodeActionInserterFactory : IdentifiableDatabaseInserterFactory<Request, Inserter>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override string TableName => "edit_own_node_action";
}
internal sealed class EditOwnNodeActionInserter : IdentifiableDatabaseInserter<Request>
{
    public EditOwnNodeActionInserter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeTypeId, request.NodeTypeId),
        };
    }
}
