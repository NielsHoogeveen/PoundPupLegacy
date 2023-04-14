using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Deleters;

using Request = NodeTermDeleterRequest;
using Factory = NodeTermDeleterFactory;
using Deleter = NodeTermDeleter;

public record NodeTermDeleterRequest: IRequest
{
    public required int NodeId { get; init; }
    public required int TermId { get; init; }
}

internal sealed class NodeTermDeleterFactory : DatabaseDeleterFactory<Request, Deleter>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableIntegerDatabaseParameter TermId = new() { Name = "term_id" };

    public override string Sql => SQL;  

    const string SQL = $"""
        delete from node_term
        where node_id = @node_id and term_id = @term_id;
        """;
}
internal sealed class NodeTermDeleter : DatabaseDeleter<Request>
{
    public NodeTermDeleter(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Factory.NodeId, request.NodeId),
            ParameterValue.Create(Factory.TermId, request.TermId),
        };
    }
}
