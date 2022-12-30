namespace PoundPupLegacy.Model;

public record BasicPerson : Person
{
    public required int Id { get; set; }
    public required int UserId { get; init; }
    public required DateTime Created { get; init; }
    public required DateTime Changed { get; init; }
    public required string Title { get; init; }
    public required int Status { get; init; }
    public required int NodeTypeId { get; init; }
    public required DateTime? DateOfBirth { get; init; }
    public required DateTime? DateOfDeath { get; init; }
    public required int? FileIdPortrait { get; init; }
    public required bool IsTopic { get; init; }

}
