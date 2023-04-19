﻿namespace PoundPupLegacy.CreateModel.Inserters;

using Request = SimpleTextNode;

internal sealed class SimpleTextNodeInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableStringDatabaseParameter Text = new() { Name = "text" };
    private static readonly NonNullableStringDatabaseParameter Teaser = new() { Name = "teaser" };

    public override string TableName => "simple_text_node";
    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(Text, request.Text),
            ParameterValue.Create(Teaser, request.Teaser),
        };
    }
}
