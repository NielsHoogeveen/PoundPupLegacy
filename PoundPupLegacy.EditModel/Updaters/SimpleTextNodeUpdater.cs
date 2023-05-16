﻿namespace PoundPupLegacy.EditModel.Updaters;

using Request = SimpleTextNodeUpdaterRequest;

public sealed record SimpleTextNodeUpdaterRequest : IRequest
{
    public required int NodeId { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public required string Teaser { get; init; }
}

internal sealed class SimpleTextNodeUpdaterFactory : DatabaseUpdaterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    private static readonly NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    private static readonly NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };
    private static readonly NonNullableStringDatabaseParameter Title = new() { Name = "title" };

    public override string Sql => $"""
        update node set title=@title
        where id = @node_id;
        update simple_text_node set text=@text, teaser=@teaser
        where id = @node_id;
        """;
    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(Title, request.Title),
            ParameterValue.Create(NodeId, request.NodeId),
            ParameterValue.Create(Text, request.Text),
            ParameterValue.Create(Teaser, request.Teaser),
        };
    }
}

