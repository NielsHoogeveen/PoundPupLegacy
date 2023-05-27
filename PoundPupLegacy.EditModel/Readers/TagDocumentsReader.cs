namespace PoundPupLegacy.EditModel.Readers;

using Request = TagDocumentsReaderRequest;
using SearchOption = Common.SearchOption;

public sealed record TagDocumentsReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required string SearchString { get; init; }
    public required int[] NodeTypeIds { get; init; }
}

internal sealed class TagDocumentsReaderFactory : EnumerableDatabaseReaderFactory<Request, NodeTerm.NewNodeTerm>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly SearchOptionDatabaseParameter SearchOptionParameter = new() { Name = "search_option" };
    private static readonly NonNullableStringDatabaseParameter SearchStringParameter = new() { Name = "search_string" };
    private static readonly NonNullableIntegerArrayDatabaseParameter NodeTypeIds = new() { Name = "node_type_ids" };

    private static readonly NullableIntValueReader NodeIdReader = new() { Name = "node_id" };
    private static readonly IntValueReader NodeTypeIdReader = new() { Name = "node_type_id" };
    private static readonly IntValueReader TermIdReader = new() { Name = "term_id" };
    private static readonly StringValueReader NameReader = new() { Name = "name" };

    public override string Sql => SQL;

    const string SQL = """
        select
        distinct
        id term_id,
        name,
        node_type_id 
        from(
            select
            t.id,
            t.name,
            n.node_type_id
            from term t
            join node n on n.id = t.nameable_id
            join system_group sg on sg.id = 0
            where t.vocabulary_id = sg.vocabulary_id_tagging and t.name = @search_string
            and n.node_type_id = any(@node_type_ids)
            union
            select
            t.id,
            t.name,
            n.node_type_id
            from term t
            join node n on n.id = t.nameable_id
            join system_group sg on sg.id = 0
            where t.vocabulary_id = sg.vocabulary_id_tagging and t.name ilike @search_option
            and n.node_type_id = any(@node_type_ids)
            LIMIT 50
        ) x
        """;


    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(SearchStringParameter, request.SearchString),
            ParameterValue.Create(SearchOptionParameter, (request.SearchString, SearchOption.StartsWith)),
            ParameterValue.Create(NodeTypeIds, request.NodeTypeIds)
        };
    }

    protected override NodeTerm.NewNodeTerm Read(NpgsqlDataReader reader)
    {
        return new NodeTerm.NewNodeTerm {
            Name = NameReader.GetValue(reader),
            TermId = TermIdReader.GetValue(reader),
            NodeTypeId = NodeTypeIdReader.GetValue(reader)
        };
    }
}
