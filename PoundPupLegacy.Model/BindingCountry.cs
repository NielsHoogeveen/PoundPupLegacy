namespace PoundPupLegacy.Model;

public record BindingCountry : Node, TopLevelCountry
{
    public required int Id { get; set; }
    public required int UserId { get; init; }
    public required DateTime Created { get; init; }
    public required DateTime Changed { get; init; }
    public required string Title { get; init; }
    public required int Status { get; init; }
    public required int NodeTypeId { get; init; }
    public required bool IsTerm { get; init; }
    public required int GlobalRegionId { get; init; }
    public required string ISO3166_1_Code { get; init; }
}


