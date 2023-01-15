namespace PoundPupLegacy.Model;

public record CreateNodeAction: Action
{
    public required int? Id { get; set; }

    public required int NodeTypeId { get; init; }

}
