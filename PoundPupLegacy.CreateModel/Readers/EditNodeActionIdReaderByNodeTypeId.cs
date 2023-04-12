namespace PoundPupLegacy.CreateModel.Readers;

using Factory = EditNodeActionIdReaderByNodeTypeIdFactory;
using Reader = EditNodeActionIdReaderByNodeTypeId;

public sealed class EditNodeActionIdReaderByNodeTypeIdFactory : DatabaseReaderFactory<Reader>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT id FROM edit_node_action WHERE node_type_id = @node_type_id
        """;
}

public sealed class EditNodeActionIdReaderByNodeTypeId : IntDatabaseReader<int>
{
    internal EditNodeActionIdReaderByNodeTypeId(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(int request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeTypeId, request)
        };
    }

    protected override IntValueReader IntValueReader => Factory.IdReader;

    protected override string GetErrorMessage(int request)
    {
        return $"edit node action cannot be found for node type { request }";
    }
}
