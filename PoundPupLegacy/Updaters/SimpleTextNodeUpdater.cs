using Npgsql;
using PoundPupLegacy.Common;
using System.Data;

namespace PoundPupLegacy.Updaters;

internal sealed class SimpleTextNodeUpdaterFactory : DatabaseUpdaterFactory<SimpleTextNodeUpdater>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() {
        Name = "node_id"
    };
    internal static NonNullableStringDatabaseParameter Text = new() {
        Name = "text"
    };
    internal static NonNullableStringDatabaseParameter Teaser = new() {
        Name = "teaser"
    };
    internal static NonNullableStringDatabaseParameter Title = new() {
        Name = "title"
    };

    public override string Sql => $"""
        update node set title=@title
        where id = @node_id;
        update simple_text_node set text=@text, teaser=@teaser
        where id = @node_id;
        """;  

}

internal sealed class SimpleTextNodeUpdater : DatabaseUpdater<SimpleTextNodeUpdater.Request>
{


    public SimpleTextNodeUpdater(NpgsqlCommand command) : base(command)
    {
    }

    public override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(SimpleTextNodeUpdaterFactory.Title, request.Title),
            ParameterValue.Create(SimpleTextNodeUpdaterFactory.NodeId, request.NodeId),
            ParameterValue.Create(SimpleTextNodeUpdaterFactory.Text, request.Text),
            ParameterValue.Create(SimpleTextNodeUpdaterFactory.Teaser, request.Teaser),
        };
    }

    public record Request
    {
        public required int NodeId { get; init; }
        public required string Title { get; init; }
        public required string Text { get; init; }
        public required string Teaser { get; init; }
   }
}
