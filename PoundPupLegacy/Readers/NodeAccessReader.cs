namespace PoundPupLegacy.Readers;

using Npgsql;
using PoundPupLegacy.Common;
using PoundPupLegacy.Models;
using Request = NodeAccessReaderRequest;

public sealed record NodeAccessReaderRequest : IRequest
{
    public required int NodeId { get; init; }
}

internal sealed class NodeAccessReaderFactory : EnumerableDatabaseReaderFactory<Request, NodeAccess>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeIdParameter = new() { Name = "node_id" };

    public override string Sql => SQL;

    const string SQL = $"""
            select
            p.name,
            na.date_time
            from node_access na
            join publisher p on p.id = na.user_id
            where node_id = @node_id
            order by na.date_time desc
            limit 50
            """;

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeIdParameter, request.NodeId),
        };
    }

    protected override NodeAccess Read(NpgsqlDataReader reader)
    {
        return new NodeAccess { Name = reader.GetString(0), DateTime = reader.GetDateTime(1) };
    }
}
