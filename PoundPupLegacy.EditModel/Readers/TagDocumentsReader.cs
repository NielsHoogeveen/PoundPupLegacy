using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.EditModel.Readers;

public class TagDocumentsReaderFactory : DatabaseReaderFactory<TagDocumentsReader>
{
    internal static NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    internal static NonNullableStringDatabaseParameter SearchString = new() { Name = "search_string" };

    public override string Sql => SQL;

    const string SQL = """
        select
        distinct
        *
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
public class TagDocumentsReader : EnumerableDatabaseReader<TagDocumentsReader.TagDocumentsRequest, Tag>
{
    public record TagDocumentsRequest
    {
        public required int? NodeId { get; init; }
        public required int TenantId { get; init; }
        public required string SearchString { get; init; }

    }
    internal TagDocumentsReader(NpgsqlCommand command) : base(command)
    {
    }
    public override async IAsyncEnumerable<Tag> ReadAsync(TagDocumentsRequest request)
    {
        _command.Parameters["tenant_id"].Value = request.TenantId;
        _command.Parameters["search_string"].Value = $"%{request.SearchString}%";
        await using var reader = await _command.ExecuteReaderAsync();
        while (await reader.ReadAsync()) {
            yield return new Tag {
                Name = reader.GetString(1),
                NodeId = request.NodeId,
                TermId = reader.GetInt32(0),
                HasBeenDeleted = false,
                IsStored = false,
            };
        }
    }

}
