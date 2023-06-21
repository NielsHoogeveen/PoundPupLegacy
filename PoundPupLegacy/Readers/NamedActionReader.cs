using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;

namespace PoundPupLegacy.Readers;

using Request = NamedActionsReaderRequest;

public sealed record NamedActionsReaderRequest : IRequest
{
}
internal sealed class NamedActionsReaderFactory : EnumerableDatabaseReaderFactory<Request, NamedAction>
{
    private static readonly IntValueReader UserIdReader = new() { Name = "user_id" };
    private static readonly IntValueReader TenantIdReader = new() { Name = "tenant_id" };
    private static readonly StringValueReader NameReader = new() { Name = "name" };

    public override string Sql => SQL;

    const string SQL = """
        select
        t.id tenant_id,
        uguru.user_id,
        na.name
        from tenant t
        join user_group ug on ug.id = t.id
        join user_group_user_role_user uguru on uguru.user_group_id = ug.id and uguru.user_role_id = ug.administrator_role_id
        join named_action na on 1=1
        union
        select
        t.id tenant_id,
        uguru.user_id,
        na.name
        from tenant t
        join user_group ug on ug.id = t.id
        join user_group_user_role_user uguru on uguru.user_group_id = ug.id
        join named_action na on 1=1
        join access_role_privilege ap on ap.access_role_id = uguru.user_role_id and ap.action_id = na.id
        """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] { };
    }

    protected override NamedAction Read(NpgsqlDataReader reader)
    {
        return new NamedAction {
            UserId = UserIdReader.GetValue(reader),
            TenantId = TenantIdReader.GetValue(reader),
            Name = NameReader.GetValue(reader),
        };
    }
}
