namespace PoundPupLegacy.Admin.View;

public record EditNodeActionPrivilege
{
    public required int ActionId { get; init; }

    public required int NodeTypeId { get; init; }

    public required string NodeTypeName { get; init; }

}
