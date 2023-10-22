namespace PoundPupLegacy.ViewModel.Readers;

using PoundPupLegacy.ViewModel.Models;
using Request = FileDocumentReaderRequest;

public sealed class FileDocumentReaderRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int UserId { get; init; }
    public required int FileId { get; init; }
}

internal sealed class FileDocumentReaderFactory : SingleItemDatabaseReaderFactory<Request, File>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantIdParameter = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter UserIdParameter = new() { Name = "user_id" };
    private static readonly NonNullableIntegerDatabaseParameter FileIdParameter = new() { Name = "file_id" };

    private static readonly FieldValueReader<File> DocumentReader = new() { Name = "document" };

    public override string Sql => SQL;

    const string SQL = $"""
        with
        {SharedSql.ACCESSIBLE_PUBLICATIONS_STATUS}
        select
            jsonb_build_object(
                'Id', 
                id,
                'Name', 
                name,
                'MimeType', 
                mime_type,
                'Path', 
                path,
                'Size', 
                size
            ) document
        from(
            select
                id,
                path,
                name,
                mime_type,
                size,
                true can_be_accessed
            from(
                SELECT 
                    f.id,
                    f.path,
                    f.name,
                    f.mime_type,
                    f.size
                FROM public.file f
                join node_file nf on nf.file_id = f.id
                join tenant_node tn on tn.node_id = nf.node_id 
                where tn.tenant_id = @tenant_id and f.id = @file_id
                and tn.publication_status_id in 
                (
                    select 
                    id 
                    from accessible_publication_status 
                    where tenant_id = tn.tenant_id 
                    and (
                        subgroup_id = tn.subgroup_id 
                        or subgroup_id is null and tn.subgroup_id is null
                    )
                )
                union
                select
                    f.id,
                    f.path,
                    f.name,
                    f.mime_type,
                    f.size
                from "file" f
                left join node_file nf on nf.file_id = f.id
                where f.id = @file_id
                and nf.file_id is null
            ) x
        ) x
        group by id,
        path,
        name,
        mime_type,
        size
        having every(can_be_accessed)
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantIdParameter, request.TenantId),
            ParameterValue.Create(UserIdParameter, request.UserId),
            ParameterValue.Create(FileIdParameter, request.FileId),
        };
    }

    protected override File Read(NpgsqlDataReader reader)
    {
        return DocumentReader.GetValue(reader);
    }
}
