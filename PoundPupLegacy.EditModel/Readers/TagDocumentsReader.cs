namespace PoundPupLegacy.EditModel.Readers;

using Request = TagDocumentsReaderRequest;

public sealed record TagDocumentsReaderRequest : IRequest
{
    public required int NodeId { get; init; }
    public required int TenantId { get; init; }
    public required string SearchString { get; init; }
}

internal sealed class TagDocumentsReaderFactory : EnumerableDatabaseReaderFactory<Request, Tag>
{
    internal static readonly NonNullableIntegerDatabaseParameter NodeIdParameter = new() { Name = "node_id" };
    internal static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    internal static readonly NonNullableStringDatabaseParameter SearchStringParameter = new() { Name = "search_string" };

    internal static readonly IntValueReader NodeIdReader = new() { Name = "node_id" };
    internal static readonly IntValueReader TermIdReader = new() { Name = "term_id" };
    internal static readonly StringValueReader NameReader = new() { Name = "name" };

    public override string Sql => SQL;

    const string SQL = """
        select
        distinct
        id term_id,
        name,
        @node_id node_id
        from(
            select
            t.id,
            t.name
            from term t
            join tenant tt on tt.id = @tenant_id
            where t.vocabulary_id = tt.vocabulary_id_tagging and t.name = @search_string
            union
            select
            t.id,
            t.name
            from term t
            join tenant tt on tt.id = @tenant_id
            where t.vocabulary_id = tt.vocabulary_id_tagging and t.name ilike @search_string
            LIMIT 50
        ) x
        """;


    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeIdParameter, request.NodeId),
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(SearchStringParameter, request.SearchString)
        };
    }

    protected override Tag Read(NpgsqlDataReader reader)
    {
        return new Tag {
            Name = NameReader.GetValue(reader),
            NodeId = NodeIdReader.GetValue(reader),
            TermId = TermIdReader.GetValue(reader),
            HasBeenDeleted = false,
            IsStored = false,
        };
    }

}
