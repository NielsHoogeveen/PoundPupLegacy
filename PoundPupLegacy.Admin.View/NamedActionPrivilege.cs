namespace PoundPupLegacy.Admin.View;

public record NamedActionPrivilege
{
    public required int ActionId { get; init; }

    public required string Name { get; init; }

}
