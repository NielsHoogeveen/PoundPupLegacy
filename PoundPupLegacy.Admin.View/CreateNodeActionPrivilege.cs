namespace PoundPupLegacy.Admin.View;

public record CreateNodeActionPrivilege
{
    public required int ActionId { get; init; }

    public required int NodeTypeId { get; init; }

    public required string NodeTypeName { get; init; }

    public required ActionMenuItem? MenuItem { get; init; }

}
