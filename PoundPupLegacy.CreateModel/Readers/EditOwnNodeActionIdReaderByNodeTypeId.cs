namespace PoundPupLegacy.CreateModel.Readers;

using Factory = EditOwnNodeActionIdReaderByNodeTypeIdFactory;
using Reader = EditOwnNodeActionIdReaderByNodeTypeId;

public sealed class EditOwnNodeActionIdReaderByNodeTypeIdFactory : DatabaseReaderFactory<Reader>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;
    const string SQL = """
        SELECT id FROM edit_own_node_action WHERE node_type_id = @node_type_id
        """;
}
public sealed class EditOwnNodeActionIdReaderByNodeTypeId : IntDatabaseReader<int>
{
    internal EditOwnNodeActionIdReaderByNodeTypeId(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(int request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeTypeId, request)
        };
    }

    protected override IntValueReader IntValueReader => Factory.IdReader;

    protected override string GetErrorMessage(int request)
    {
        return $"edit own node action cannot be found for node type {request}";
    }
}
