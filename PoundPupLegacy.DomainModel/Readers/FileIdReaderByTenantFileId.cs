﻿namespace PoundPupLegacy.DomainModel.Readers;

using Request = FileIdReaderByTenantFileIdRequest;

public sealed class FileIdReaderByTenantFileIdRequest : IRequest
{
    public required int TenantId { get; init; }
    public required int TenantFileId { get; init; }
}
internal sealed class FileIdReaderByTenantFileIdFactory : IntDatabaseReaderFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter TenantId = new() { Name = "tenant_id" };
    private static readonly NonNullableIntegerDatabaseParameter TenantFileId = new() { Name = "tenant_file_id" };

    private static readonly IntValueReader IdReader = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = """
        SELECT file_id id FROM tenant_file WHERE tenant_id = @tenant_id and tenant_file_id = @tenant_file_id
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(TenantId, request.TenantId),
            ParameterValue.Create(TenantFileId, request.TenantFileId)
        };
    }

    protected override string GetErrorMessage(Request request)
    {
        return $"File id cannot be found for tenant {request.TenantId} and file id {request.TenantFileId}";
    }

    protected override IntValueReader IntValueReader => IdReader;
}
