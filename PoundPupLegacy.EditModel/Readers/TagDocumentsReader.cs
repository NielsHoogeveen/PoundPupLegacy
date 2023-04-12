namespace PoundPupLegacy.EditModel.Readers;

using Factory = TagDocumentsReaderFactory;
using Reader = TagDocumentsReader;

public class TagDocumentsReaderFactory : DatabaseReaderFactory<Reader>
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

}
public class TagDocumentsReader : EnumerableDatabaseReader<Reader.Request, Tag>
{
    public record Request
    {
        public required int NodeId { get; init; }
        public required int TenantId { get; init; }
        public required string SearchString { get; init; }

    }
    internal TagDocumentsReader(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeIdParameter, request.NodeId),
            ParameterValue.Create(Factory.TenantIdParameter, request.TenantId),
            ParameterValue.Create(Factory.SearchStringParameter, request.SearchString)
        };
    }

    protected override Tag Read(NpgsqlDataReader reader)
    {
        return new Tag {
            Name = Factory.NameReader.GetValue(reader),
            NodeId = Factory.NodeIdReader.GetValue(reader),
            TermId = Factory.TermIdReader.GetValue(reader),
            HasBeenDeleted = false,
            IsStored = false,
        };
    }

}
