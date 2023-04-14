﻿using Npgsql;
using PoundPupLegacy.Common;

namespace PoundPupLegacy.Updaters;

using Request = SimpleTextNodeUpdaterRequest;
using Factory = SimpleTextNodeUpdaterFactory;
using Updater = SimpleTextNodeUpdater;

public record SimpleTextNodeUpdaterRequest : IRequest
{
    public required int NodeId { get; init; }
    public required string Title { get; init; }
    public required string Text { get; init; }
    public required string Teaser { get; init; }
}

internal sealed class SimpleTextNodeUpdaterFactory : DatabaseUpdaterFactory<Request,Updater>
{
    internal static NonNullableIntegerDatabaseParameter NodeId = new() { Name = "node_id" };
    internal static NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    internal static NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };
    internal static NonNullableStringDatabaseParameter Title = new() { Name = "title" };

    public override string Sql => $"""
        update node set title=@title
        where id = @node_id;
        update simple_text_node set text=@text, teaser=@teaser
        where id = @node_id;
        """;

}

internal sealed class SimpleTextNodeUpdater : DatabaseUpdater<Request>
{


    public SimpleTextNodeUpdater(NpgsqlCommand command) : base(command)
    {
    }

    protected override IEnumerable<ParameterValue> GetParameterValues(Request request)
    {
        return new List<ParameterValue> {
            ParameterValue.Create(Factory.Title, request.Title),
            ParameterValue.Create(Factory.NodeId, request.NodeId),
            ParameterValue.Create(Factory.Text, request.Text),
            ParameterValue.Create(Factory.Teaser, request.Teaser),
        };
    }
}
