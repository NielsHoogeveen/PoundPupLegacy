namespace PoundPupLegacy.Admin.View;

public record BasicActionPrivilege
{
    public required int ActionId { get; init; }

    public required string Path { get; init; }

    public required string? Description { get; init; }

    public required ActionMenuItem? MenuItem { get; init; }
}
