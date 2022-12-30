namespace PoundPupLegacy.Model;

public record WrongfulMedicationCase : Case
{
    public required int Id { get; set; }
    public required int UserId { get; init; }
    public required DateTime Created { get; init; }
    public required DateTime Changed { get; init; }
    public required string Title { get; init; }
    public required int Status { get; init; }
    public required int NodeTypeId { get; init; }
    public required string Description { get; init; }
    public required DateTimeRange? Date { get; init; }
    public required bool IsTopic { get; init; }

}
