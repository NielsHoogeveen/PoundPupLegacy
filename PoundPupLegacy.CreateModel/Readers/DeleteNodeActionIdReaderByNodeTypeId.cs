namespace PoundPupLegacy.CreateModel.Readers;

using Factory = DeleteNodeActionIdReaderByNodeTypeIdFactory;
using Reader = DeleteNodeActionIdReaderByNodeTypeId;

public sealed class DeleteNodeActionIdReaderByNodeTypeIdFactory : DatabaseReaderFactory<Reader>
{
    internal static NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    internal static IntValueReader IdReader = new() { Name = "id" };
    public override string Sql => SQL;
    
    private const string SQL = """
        SELECT id FROM delete_node_action WHERE node_type_id = @node_type_id
        """;
}

public sealed class DeleteNodeActionIdReaderByNodeTypeId : IntDatabaseReader<int>
{

    internal DeleteNodeActionIdReaderByNodeTypeId(NpgsqlCommand command) : base(command) { }

    protected override IEnumerable<ParameterValue> GetParameterValues(int request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeTypeId, request)
        };
    }

    protected override IntValueReader IntValueReader => Factory.IdReader;

    protected override string GetErrorMessage(int request)
    {
        return $"delete node action cannot be found for node type {request}";
    }
}
