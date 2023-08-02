﻿namespace PoundPupLegacy.DomainModel.Inserters;

using Request = EditOwnNodeAction;

internal sealed class EditOwnNodeActionInserterFactory : IdentifiableDatabaseInserterFactory<Request>
{
    private static readonly NonNullableIntegerDatabaseParameter NodeTypeId = new() { Name = "node_type_id" };

    public override string TableName => "edit_own_node_action";

    protected override IEnumerable<ParameterValue> GetNonIdParameterValues(Request request)
    {
        return new ParameterValue[] {
            ParameterValue.Create(NodeTypeId, request.NodeTypeId),
        };
    }
}