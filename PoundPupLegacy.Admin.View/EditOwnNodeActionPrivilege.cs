namespace PoundPupLegacy.Admin.View;

public record EditOwnNodeActionPrivilege
{
    public required int ActionId { get; init; }

    public required int NodeTypeId { get; init; }

    public required string NodeTypeName { get; init; }

}
