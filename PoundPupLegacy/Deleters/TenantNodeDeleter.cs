using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Deleters;

using Request = TenantNodeDeleterRequest;
using Factory = TenantNodeDeleterFactory;
using Deleter = TenantNodeDeleter;

public record TenantNodeDeleterRequest: IRequest
{
    public required int Id { get; init; }
}

internal sealed class TenantNodeDeleterFactory : DatabaseDeleterFactory<Request,Deleter>
{

    public static NonNullableIntegerDatabaseParameter Id = new() { Name = "id" };

    public override string Sql => SQL;

    const string SQL = $"""
        delete from tenant_node
        where id = @id;
        """;
}
internal sealed class TenantNodeDeleter : DatabaseDeleter<Request>
{
    public TenantNodeDeleter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.Id, request.Id),
        };
    }
}
